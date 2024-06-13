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
    private Coroutine onCorutine;
    void Start()
    {
        if (Instant == null)
        {
            Instant = this;
        }
        onCorutine = null;
        button = loginButton.GetComponent<Button>();
        loginText = loginButton.GetComponentInChildren<TMP_Text>();
    }
    void Update()
    {
        if (userInput.text == "" || passInput.text == "" || Client.instant.IsConnect || onCorutine != null)
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

    private float dotSpeed = 0.3f;

    private IEnumerator WaitForResponAnimation()
    {
        string[] dots = { ".", "..", "..." };
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                loginText.text = $"Wait{dots[i]}";
                yield return new WaitForSeconds(dotSpeed);
            }
        }
    }

    private IEnumerator ErrorLoginAnimation(string text)
    {
        loginText.text = text;
        yield return new WaitForSeconds(1.5f);
        loginText.text = "Login";
        onCorutine = null;
    }

    private IEnumerator ConnectStatusAnimation()
    {
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
        onCorutine = null;
    }

    public void PlayConnectStatus()
    {
        try
        {
            Stop();
            onCorutine = StartCoroutine(ConnectStatusAnimation());
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void PlayErrorLoginAnimation(string s)
    {
        //แก้ด้วยน้ะ Error อยู่
        try
        {
            Stop();
            onCorutine = StartCoroutine(ErrorLoginAnimation(s));
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

    }

    private void Stop()
    {
        if (onCorutine == null)
        {
            return;
        }
        StopCoroutine(onCorutine);
        onCorutine = null;
    }


}
