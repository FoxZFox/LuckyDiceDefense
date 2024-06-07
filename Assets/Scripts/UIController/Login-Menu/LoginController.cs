using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.Remoting.Contexts;
public class LoginController : MonoBehaviour
{
    public static LoginController Instant;
    [SerializeField] private GameObject loginButton;
    [SerializeField] private TMP_InputField userInput;
    [SerializeField] private TMP_InputField passInput;
    private Button button;
    private TMP_Text loginText;

    void Start()
    {
        if (Instant == null)
        {
            Instant = this;
        }
        button = loginButton.GetComponent<Button>();
        loginText = loginButton.GetComponentInChildren<TMP_Text>();
    }
    void Update()
    {
        if (userInput.text == "" || passInput.text == "" || Client.instant.IsConnect || Client.instant.StartCon || onCorutine != null)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void Login()
    {
        Networkmanager.Instant.Login(userInput.text, passInput.text);
        onCorutine = StartCoroutine(WaitForResponAnimation());
    }

    [ContextMenu("Fill U&P")]
    public void Fill()
    {
        userInput.text = "test1";
        passInput.text = "12345";
    }

    private IEnumerator WaitForResponAnimation()
    {
        string[] dots = { ".", "..", "..." };
        while (Client.instant.StartCon || Client.instant.CheckPacket())
        {
            for (int i = 0; i < 3; i++)
            {
                loginText.text = $"Wait{dots[i]}";
                yield return new WaitForSeconds(0.3f);
            }
        }
        if (Client.instant.account.ID >= 20000)
        {
            loginText.text = $"Connect with {userInput.text}";
        }
        else if (!Client.instant.IsConnect)
        {
            loginText.text = $"Can't connect to server";
            yield return new WaitForSeconds(1.5f);
            loginText.text = $"Login";
        }
        StopCoroutine(onCorutine);
        onCorutine = null;
    }

    public void PlayErrorLoginAnimation(string s)
    {
        if (onCorutine != null)
            StopCoroutine(onCorutine);
        onCorutine = StartCoroutine(ErrorLoginAnimation(s));
    }

    Coroutine onCorutine;
    private IEnumerator ErrorLoginAnimation(string text)
    {
        loginText.text = text;
        yield return new WaitForSeconds(1.5f);
        loginText.text = "Login";
        StopCoroutine(onCorutine);
        onCorutine = null;
    }
}
