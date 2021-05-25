using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                descripcionArma.text = "USA SUS NOTAS TANTO PARA HACER DAÑO COMO PARA PROTEGERSE DE SUS ENEMIGOS";
                miniaturaHabilidad.sprite = habilidadBadBaby;
                descripcionHabilidad.text = "ATRAE A TU ENEMIGO AL RITMO DE LA MUSICA";
                break;
            case 2:
                miniatura.sprite = miniaturaBob;
                characterName.text = "BOB OJOCOJO";
                miniaturaArma.sprite = armaBob;
                damageStat.sprite = damage2;
                descripcionArma.text = "REBOTA REBOTA Y EN TU CARA EXPLOTA";
                miniaturaHabilidad.sprite = habilidadBob;
                descripcionHabilidad.text = "JOSE VA TO CIEGO";
                break;
            case 3:
                miniatura.sprite = miniaturaCamomila;
                characterName.text = "CAMOMILA SESTIMA";
                miniaturaArma.sprite = armaCamomila;
                damageStat.sprite = damage3;
                descripcionArma.text = "PIM PAM TOMA LACASITOS";
                miniaturaHabilidad.sprite = habilidadCamomila;
                descripcionHabilidad.text = "A TODO OJETE";
                break;
            case 4:
                miniatura.sprite = miniaturaChuerk;
                characterName.text = "CHUERK CHUERK";
                miniaturaArma.sprite = armaChuerk;
                damageStat.sprite = damage1;
                descripcionArma.text = "SOMEBODY ONCE TOLD ME";
                miniaturaHabilidad.sprite = habilidadChuerk;
                descripcionHabilidad.text = "JOSE TIENE CAPAS COMO LAS CEBOLLAS";
                break;
            case 5:
                miniatura.sprite = miniaturaManolo;
                characterName.text = "MANOLO MCFLY";
                miniaturaArma.sprite = armaManolo;
                damageStat.sprite = damage2;
                descripcionArma.text = "RAFAGAS COMO EN LOS COCHES DE CHOQUE";
                miniaturaHabilidad.sprite = habilidadManolo;
                descripcionHabilidad.text = "JOSE MANOLO";
                break;
        }
    }
}
