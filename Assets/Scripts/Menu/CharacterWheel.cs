using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class CharacterWheel : MonoBehaviour
{
    public Transform leftButton;
    public Transform rightButton;

    int children;
    float degrees;

    //Posicion inicial
    float startAngle = -90;
    float degreeToRad = Mathf.PI / 180;
    float radius = 3.25f;

    //Control
    int characterSelected = 1;

    //Rotacion
    public float lerpTime = 0.1f;
    float t = 0.0f;
    public bool rotating = false;
    private bool dir = false; //False = Right, True = Left

    Vector3 startRot;
    Vector3 endRot;

    //Interfaz
    public Image miniatura;
    public Sprite miniaturaChuerk;
    public Sprite miniaturaBadBaby;
    public Sprite miniaturaCamomila;
    public Sprite miniaturaManolo;
    public Sprite miniaturaBob;
    public Text characterName;

    public Image damageStat;
    public Sprite damage1;
    public Sprite damage2;
    public Sprite damage3;

    public Image miniaturaArma;
    public Sprite armaChuerk;
    public Sprite armaBadBaby;
    public Sprite armaCamomila;
    public Sprite armaManolo;
    public Sprite armaBob;
    public Text descripcionArma;

    public Image miniaturaHabilidad;
    public Sprite habilidadChuerk;
    public Sprite habilidadBadBaby;
    public Sprite habilidadCamomila;
    public Sprite habilidadManolo;
    public Sprite habilidadBob;
    public Text descripcionHabilidad;

    private void Start()
    {
        WheelSetup();
    }

    private void WheelSetup()
    {
        if(leftButton && rightButton)
        {
            float x = leftButton.position.x + ((rightButton.position.x - leftButton.position.x) / 2.0f);

            radius = (transform.position.y - rightButton.position.y);

            transform.position.Set(x, transform.position.y, transform.position.z);
        }

        children = transform.childCount;

        float r = radius;// * scaler.scaleFactor;

        degrees = 360.0f / children;

        for (int i = 0; i < children; i++)
        {
            Vector3 aux = new Vector3(r * Mathf.Cos((startAngle + degrees * i) * degreeToRad), r * Mathf.Sin((startAngle + degrees * i) * degreeToRad), 0);

            transform.GetChild(i).position = transform.position + aux;
        }

        UpdateCharacterSelected();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    WheelSetup();
        //}

        if (rotating)
        {
            if (t < lerpTime)
            {
                t += Time.deltaTime;

                transform.eulerAngles = Vector3.Lerp(startRot, endRot, t / lerpTime);

                int pivotChildren = transform.childCount;

                for (int i = 0; i < pivotChildren; i++)
                {
                    transform.GetChild(i).SetPositionAndRotation(transform.GetChild(i).position, Quaternion.identity);
                }
            }
            else
            {
                rotating = false;
                t = 0.0f;

                if (dir)
                {
                    if (characterSelected > 1)
                        characterSelected--;
                    else
                        characterSelected = children;
                }
                else
                {
                    if (characterSelected < children)
                        characterSelected++;
                    else
                        characterSelected = 1;
                }

                UpdateCharacterSelected();
            }
        }
    }

    public void startLerp(bool leftDir)
    {
        rotating = true;
        dir = leftDir;

        Vector3 mod = new Vector3(0, 0, 360.0f / (float)transform.childCount);

        if (!leftDir) mod = -mod;

        startRot = transform.eulerAngles;
        endRot = startRot + mod;
    }

    public void UpdateCharacterSelected()
    {
        if(Mirror.NetworkManager.singleton)
            ((ExtendedNetworkManager)Mirror.NetworkManager.singleton).playerSelection = characterSelected - 1;

        switch (characterSelected)
        {
            case 1:
                miniatura.sprite = miniaturaBadBaby;
                characterName.text = "BAD BABY";
                miniaturaArma.sprite = armaBadBaby;
                damageStat.sprite = damage3;
                miniaturaHabilidad.sprite = habilidadBadBaby;

                var test_Bad = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ArmaBadBaby");

                if (test_Bad.IsDone)
                {
                    descripcionArma.text = test_Bad.Result;
                }
                else
                    test_Bad.Completed += (test1) => descripcionArma.text = test_Bad.Result;

                var test_Bad_A = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "HabilidadBadBaby");

                if (test_Bad_A.IsDone)
                {
                    descripcionHabilidad.text = test_Bad_A.Result;
                }
                else
                    test_Bad_A.Completed += (test1) => descripcionHabilidad.text = test_Bad_A.Result;

                break;
            case 2:
                miniatura.sprite = miniaturaBob;
                characterName.text = "BOB OJOCOJO";
                miniaturaArma.sprite = armaBob;
                damageStat.sprite = damage2;
                miniaturaHabilidad.sprite = habilidadBob;

                var test_Bob = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ArmaBob");

                if (test_Bob.IsDone)
                {
                    descripcionArma.text = test_Bob.Result;
                }
                else
                    test_Bob.Completed += (test1) => descripcionArma.text = test_Bob.Result;

                var test_Bob_A = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "HabilidadBob");

                if (test_Bob_A.IsDone)
                {
                    descripcionHabilidad.text = test_Bob_A.Result;
                }
                else
                    test_Bob_A.Completed += (test1) => descripcionHabilidad.text = test_Bob_A.Result;

                break;
            case 3:
                miniatura.sprite = miniaturaCamomila;
                characterName.text = "CAMOMILA SESTIMA";
                miniaturaArma.sprite = armaCamomila;
                damageStat.sprite = damage3;
                miniaturaHabilidad.sprite = habilidadCamomila;

                var test_Camomila = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ArmaCamomila");

                if (test_Camomila.IsDone)
                {
                    descripcionArma.text = test_Camomila.Result;
                }
                else
                    test_Camomila.Completed += (test1) => descripcionArma.text = test_Camomila.Result;

                var test_Camomila_A = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "HabilidadCamomila");

                if (test_Camomila_A.IsDone)
                {
                    descripcionHabilidad.text = test_Camomila_A.Result;
                }
                else
                    test_Camomila_A.Completed += (test1) => descripcionHabilidad.text = test_Camomila_A.Result;

                break;
            case 4:
                miniatura.sprite = miniaturaChuerk;
                characterName.text = "CHUERK CHUERK";
                miniaturaArma.sprite = armaChuerk;
                damageStat.sprite = damage1;
                miniaturaHabilidad.sprite = habilidadChuerk;

                var test_Chuerk = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ArmaChuerk");

                if (test_Chuerk.IsDone)
                {
                    descripcionArma.text = test_Chuerk.Result;
                }
                else
                    test_Chuerk.Completed += (test1) => descripcionArma.text = test_Chuerk.Result;

                var test_Chuerk_A = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "HabilidadChuerk");

                if (test_Chuerk_A.IsDone)
                {
                    descripcionHabilidad.text = test_Chuerk_A.Result;
                }
                else
                    test_Chuerk_A.Completed += (test1) => descripcionHabilidad.text = test_Chuerk_A.Result;

                break;
            case 5:
                miniatura.sprite = miniaturaManolo;
                characterName.text = "MANOLO MCFLY";
                miniaturaArma.sprite = armaManolo;
                damageStat.sprite = damage2;
                miniaturaHabilidad.sprite = habilidadManolo;

                var test_Manolo = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "ArmaManolo");

                if (test_Manolo.IsDone)
                {
                    descripcionArma.text = test_Manolo.Result;
                }
                else
                    test_Manolo.Completed += (test1) => descripcionArma.text = test_Manolo.Result;

                var test_Manolo_A = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI Text", "HabilidadManolo");

                if (test_Manolo_A.IsDone)
                {
                    descripcionHabilidad.text = test_Manolo_A.Result;
                }
                else
                    test_Manolo_A.Completed += (test1) => descripcionHabilidad.text = test_Manolo_A.Result;

                break;
        }
    }
}
