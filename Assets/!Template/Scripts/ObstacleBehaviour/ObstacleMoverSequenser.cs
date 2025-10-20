using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ObstacleMoverSequenser : MonoBehaviour
{
    [SerializeField] private MoverBehaviourSequence[] behaviourSequences;
    [SerializeField] private bool playOnStart = true;

    private void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play()
    {
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        while (true)
        {
            for (int i = 0; i < behaviourSequences.Length; i++)
            {
                yield return StartCoroutine(PlaySequencesCoroutine(behaviourSequences[i]));
            }
            yield return null;
        }
    }

    private IEnumerator PlaySequencesCoroutine(MoverBehaviourSequence behaviourSequence)
    {
        // ����� ������-���� �� ID (��� GameObject)
        var target = transform;
        Tween tween = null;

        switch (behaviourSequence.behaviourType)
        {
            case MoverBehaviourType.Wait:
                // ������ �� ���������, ������ ��� ������ �����
                if (behaviourSequence.waitToPlay)
                {
                    yield return new WaitForSeconds(behaviourSequence.duration);
                }
                // ���� waitToPlay == false � ����� ���������� ����������
                yield break;

            case MoverBehaviourType.MovePosition:
                tween = target.DOMove(behaviourSequence.endPosition, behaviourSequence.duration)
                              .SetEase(behaviourSequence.ease);
                break;

            case MoverBehaviourType.MoveLocalPosition:
                tween = target.DOLocalMove(behaviourSequence.endLocalPosition, behaviourSequence.duration)
                              .SetEase(behaviourSequence.ease);
                break;

            case MoverBehaviourType.MoveXPosition:
                tween = target.DOMoveX(behaviourSequence.endX, behaviourSequence.duration)
                              .SetEase(behaviourSequence.ease);
                break;

            case MoverBehaviourType.MoveYPosition:
                tween = target.DOMoveY(behaviourSequence.endY, behaviourSequence.duration)
                              .SetEase(behaviourSequence.ease);
                break;

            case MoverBehaviourType.MoveZPosition:
                tween = target.DOMoveZ(behaviourSequence.endZ, behaviourSequence.duration)
                              .SetEase(behaviourSequence.ease);
                break;

            case MoverBehaviourType.Rotate:
                tween = target.DORotate(behaviourSequence.endRotation, behaviourSequence.duration)
                              .SetEase(behaviourSequence.ease);
                break;

            case MoverBehaviourType.Scale:
                tween = target.DOScale(behaviourSequence.endScale, behaviourSequence.duration)
                              .SetEase(behaviourSequence.ease);
                break;

            default:
                Debug.LogWarning($"[ObstacleMoverSequenser] Unsupported behaviourType '{behaviourSequence.behaviourType}'.");
                yield break;
        }

        if (behaviourSequence.waitToPlay)
        {
            // ��� ���������� �����
            yield return tween.WaitForCompletion();
        }
        // ���� waitToPlay == false, ����� ���������� ����������
    }
}

public enum MoverBehaviourType
{
    Wait,
    MovePosition,
    MoveLocalPosition,
    MoveXPosition,
    MoveYPosition,
    MoveZPosition,
    Rotate,
    Scale
}

[System.Serializable]
public class MoverBehaviourSequence
{
    [Tooltip("��� GameObject (ID), � �������� ����������� ���������")]
    public string ID;

    [Tooltip("��� tween-��������")]
    public MoverBehaviourType behaviourType = MoverBehaviourType.Wait;

    [Tooltip("������������ �������� ��� ��������")]
    public float duration = 1f;

    [Tooltip("Ease ��� tween (��������, Ease.OutQuad)")]
    public Ease ease = Ease.Linear;

    [Space]
    [Header("��������� �������� (World Space)")]
    public Vector3 endPosition = Vector3.zero;

    [Header("��������� �������� (Local Space)")]
    public Vector3 endLocalPosition = Vector3.zero;

    [Header("��������� ��� ������ �����������")]
    public float endX;
    public float endY;
    public float endZ;

    [Header("��������� �������� � ��������")]
    public Vector3 endRotation = Vector3.zero; // Euler angles
    public Vector3 endScale = Vector3.one;

    [Space]
    [Tooltip("���� true, ��������� Sequence ��� ���������� ����� tween; ����� ����������� �����")]
    public bool waitToPlay = true;
}