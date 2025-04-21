using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // ī�޶� ȸ�� ��ũ��Ʈ
    // ��ǥ: ���콺�� �����ϸ� ī�޶� �� �������� ȸ����Ű�� �ʹ�.

    public float RotationSpeed = 15f;
    
    // ī�޶� ������ 0���������� �����Ѵٰ� ������ �����.
    private float _rotationX = 0;
    private float _rotationY = 0;
    private void Update()
    {
        // ���� ����
        // 1. ���콺 �Է��� �޴´�.(���콺�� Ŀ�� ������ ����)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //Debug.Log($"Mouse X: {mouseX}, Mouse Y; {mouseY}");
        // 2. ���콺 �Է����κ��� ȸ����ų ������ �����.
        // Todo : ���콺 ��ǥ��� ȭ�� ��ǥ���� �������� �˰�, �� �۵� �ϵ��� �Ʒ� ������ �ڵ带 ��~ ������ ������.
        //ector3 dir = new Vector3(-mouseY, mouseX, 0);
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY* RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. ī�޶� �� �������� ȸ���Ѵ�.
        // ���ο� ��ġ = ���� ��ġ + �ӵ�(�ӷ� + �ð�)
        // ���ο� ���� = ���� ���� + ȸ�� �ӵ� * �ð�
        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);


        //ȸ���� ���� ������ �ʿ��ϴ�! (-90~ 90)


        

    }
}
