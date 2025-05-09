using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText;  //결과 텍스트
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordConfirmInputField;
    public Button ConfirmButton;
}
public class UI_LoginScene : MonoBehaviour
{
    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject ResisterPanel;

    [Header("로그인")]
    public UI_InputFields LoginInputField;

    [Header("회원가입")]
    public UI_InputFields ReqisterInputFields;

    private const string PREFIX = "ID_";
    private const string SALT = "10043420";

    private void Start()
    {
        LoginPanel.SetActive(true);
        ResisterPanel.SetActive(false);
        LoginCheck();
    }
    public void OnClickGoToResisterButton()
    {
        LoginPanel.SetActive(false);
        ResisterPanel.SetActive(true);
    }

    public void OnClickGoToLoginButton()
    {
        LoginPanel.SetActive(true);
        ResisterPanel.SetActive(false);
    }

    public void OnClickResisterButton()
    {
        Resister();
    }

    public void OnClickLoginButton()
    {
        Login();
    }

    public void Login()
    {
        // 1. 아이디 입력을 확인한다.

        string id = LoginInputField.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            LoginInputField.ResultText.text = "아이디를 입력해 주세요.";
            // (ResultText 흔들어 주세요.)
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string password = LoginInputField.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            LoginInputField.ResultText.text = "비밀번호를 입력해 주세요.";
            return;
        }

        // 3. PlayerPrefs.Get을 이용해서 아이디와 비밀번호가 맞는지 확인한다.
        if (!PlayerPrefs.HasKey(PREFIX + id))
        {
            LoginInputField.ResultText.text = "아이디와 비밀번호를 확인해 주세요.";
            return;
        }

        string hashedPassword = PlayerPrefs.GetString(PREFIX + id);
        if (hashedPassword != Encryption(password + SALT))
        {
            LoginInputField.ResultText.text = "아이디와 비밀번호를 확인해 주세요.";
            return;
        }

        // 4. 맞다면 로그인
        Debug.Log("로그인 성공!");
        SceneManager.LoadScene(1);
    }

    //회원 가입
    public void Resister()
    {
        string id = ReqisterInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            ReqisterInputFields.ResultText.text = "아이디를 입력해 주세요.";
            // (ResultText 흔들어 주세요.)
            return;
        }
        //if(PlayerPrefs.GetString(id) != null)
        //{
        //    ReqisterInputFields.ResultText.text = "이미가입한 아이디가 있습니다.";
        //    return;
        //} 

        // 2. 비밀번호 입력을 확인한다.
        string password = ReqisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            ReqisterInputFields.ResultText.text = "비밀번호를 입력해 주세요.";
            return;
        }

        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호 입력과 같은지 확인한다.
        string confirm = ReqisterInputFields.PasswordConfirmInputField.text;
        if (string.IsNullOrEmpty(confirm))
        {
            ReqisterInputFields.ResultText.text = "확인 비밀번호를 입력해 주세요.";
            return;
        }
        if (confirm != password)
        {
            ReqisterInputFields.ResultText.text = "비밀번호가 다릅니다.";
        }

        // 4. PlayerPrefs를 이용해서 아이디와 비밀번호를 저장한다.
        // (선택) -> 비밀번호를 암호화 햇서 저장하세요.

        PlayerPrefs.SetString(PREFIX + id, Encryption(password + SALT));
        LoginInputField.ResultText.text = "아이디가 생성되었습니다.";

        // 5. 로그인 창으로 돌아간다. (이때 아이디는 자동 입력되어 있다.)
        LoginInputField.IDInputField.text = id;
        OnClickGoToLoginButton();

    }

    public string Encryption(string text)
    {
        // 해시 암호화 알고리즘 인스턴스를 생성한다.
        SHA256 sha256 = SHA256.Create();

        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach (byte b in hash)
        {
            resultText += b.ToString("X2");
        }

        return resultText;
    }

    public void LoginCheck()
    {
        string id = LoginInputField.IDInputField.text;
        string password = LoginInputField.PasswordInputField.text;

        LoginInputField.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password);
    }

}
