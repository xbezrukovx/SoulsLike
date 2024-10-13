using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class TargetScript : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject pointer;
    
    private Animator _animator;
    private GameObject _target;

    private bool _isLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        pointer = Instantiate(pointer);
        pointer.SetActive(false);         
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private void TakeTarget(Transform nearestObject)
    {
        _isLocked = true;
        var virtualCamera = transform.Find("Target Locking Camera").GetComponent<CinemachineVirtualCamera>();
        virtualCamera.LookAt = nearestObject;
        _target = nearestObject.gameObject;
        pointer.SetActive(true);
        var renderer = nearestObject.GetComponent<SkinnedMeshRenderer>();
        if (renderer != null)
        {
            var center = renderer.bounds.center;
            var max = renderer.bounds.max;
            var half = max.y - center.y;
            center.y = max.y - half / 2;
            pointer.transform.position = center;
            virtualCamera.LookAt = pointer.transform;
        }
    }

    void OnTargetLock()
    {
        if (_isLocked)
        {
            _isLocked = false;
            pointer.SetActive(false);
            FollowCamera();
        }
        else
        {
            var objects = FindObjectsInView()
                .Select(obj => obj.transform)
                .ToList();
            var nearestObject = FindNearestObject(objects, Vector3.right);
            
            if (nearestObject == null) return;
            
            TakeTarget(nearestObject);
            TargetCamera();
        }
    }

    void OnRightStickLeft()
    {
        if (_isLocked) SwitchTarget(Vector3.left);
    }
    
    void OnRightStickRight()
    {
        if (_isLocked) SwitchTarget(Vector3.right);
    }
    
    private void SwitchTarget(Vector3 direction)
    {
        var visibleTargets = FindObjectsInView();
        var targets = visibleTargets
            .Select(transform => transform.gameObject)
            .Where(obj => _target != obj)
            .Select(obj => obj.transform)
            .Where(transform => positionFilter(transform, direction))
            .ToList();
        Transform closestTarget = FindNearestObject(targets, direction);

        // Если нашли ближайшую цель, устанавливаем её как текущую
        if (closestTarget != null)
        {
            TakeTarget(closestTarget);
        }
    }

    private bool positionFilter(Transform target, Vector3 direction)
    {
        var cameraPos = mainCamera.transform.position;
        var targetPos = target.position;
            
        var directionToTarget = (targetPos - cameraPos).normalized;
        var cameraDirection = mainCamera.transform.forward; 
            
        // Получаем векторы для сравнения
        var crossVector = Vector3.Cross(cameraDirection, directionToTarget);

        // Выбранная вектор и обьект находится на одной стороне 
        return direction == Vector3.right && crossVector.y > 0 || direction == Vector3.left && crossVector.y < 0;
    }
    
    private Transform FindNearestObject(List<Transform> objects, Vector3 inputDirection)
    {
        Transform nearestObject = null;
        var smallestAngle = Mathf.Infinity;  // Минимальный угол между направлением камеры и объектом
        var closestDistance = Mathf.Infinity; // Ближайшее расстояние

        // Получаем направление камеры
        var cameraDirection = mainCamera.transform.forward; // Направление камеры

        foreach (Transform obj in objects)
        {
            // Вычисляем направление от камеры к объекту
            var directionToObject = (obj.position - mainCamera.transform.position).normalized;

            // Вычисляем угол между направлением камеры и направлением к объекту
            var angle = Vector3.Angle(cameraDirection, directionToObject);
            var distance = Vector3.Distance(mainCamera.transform.position, obj.position);

            // Условия для выбора ближайшего объекта
            if (!(angle < smallestAngle) && (angle != smallestAngle || !(distance < closestDistance))) continue;
            smallestAngle = angle;
            closestDistance = distance;
            nearestObject = obj;
        }

        return nearestObject;
    }
    
    private List<Transform> FindObjectsInView()
    {
        var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        var allObjects = FindObjectsOfType<GameObject>();

        return (from obj in allObjects 
            where obj.CompareTag("Enemy") 
            let objRenderer = obj.GetComponent<Renderer>() 
            where objRenderer != null 
            where GeometryUtility.TestPlanesAABB(frustumPlanes, objRenderer.bounds) 
            select obj.transform)
            .ToList();
    }

    private void FollowCamera()
    {
        _animator.Play("FollowCam");
    }
    
    private void TargetCamera()
    {
        _animator.Play("TargetCam");
    }
}
