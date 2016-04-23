using UnityEngine;
using System.Collections;

public class MovimientosClaw : MonoBehaviour {



    public float alturaMaxima;
    public float alturaMinima;
	public float correctorSeparacionTubos;

	public GameObject Tubos;
	public GameObject Motor;
    public GameObject Gancho;
    public Animator animatorClaw;
    
    public float speed;
    public float speed_Claw;
    Rigidbody rigidBody;

    public Transform AlturaGancho;

	//Estos son simples cubos que usa para comparar los limites de movimiento


    public Transform limitadorLeft;
    public Transform limitadorRight;
    public Transform limitadorFront;
    public Transform limitadorBack;


    public bool PuedeControlarse;
     bool SoltarPremio;

    public bool[] LlegadoALaCesta;

    bool dentroDeLaCesda;

    bool bajarGanchoYSoltarPremio;

    bool subirGanchoDeLaCesta;


    // Use this for initialization
    void Start () {
        animatorClaw = Gancho.gameObject.GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        PuedeControlarse = true;
        SoltarPremio = false;
	}


    // Update is called once per frame

    void Update() {

    }


	void FixedUpdate () {

		//Fijar el motor al movimiento en X/Z del gancho
		Motor.transform.position= new Vector3(transform.position.x,Motor.transform.position.y,transform.position.z);
		Tubos.transform.position= new Vector3(Tubos.transform.position.x,Tubos.transform.position.y,Motor.transform.position.z+correctorSeparacionTubos);
			


        //soltar el premio


        if (SoltarPremio)
        {
            if (AlturaGancho.position.y <=alturaMaxima)
            {

                
                transform.Translate(0, speed_Claw * Time.deltaTime, 0);


            }
            else
            {
                LlegadoALaCesta[0] = true;
            }

            if (transform.position.x >= limitadorLeft.transform.position.x+0.5f)
            {
                
                    transform.Translate(speed * -1 * Time.deltaTime, 0, 0);

            }
            else
            {
                LlegadoALaCesta[1] = true;
            }


            if (transform.position.z >= limitadorFront.transform.position.z+0.5f)
            {
               
                    transform.Translate(0, 0, speed * -1 * Time.deltaTime);

            }
            else
            {
                LlegadoALaCesta[2] = true;
            }

            //comprobar que el gancho esta dentro de la cesta

            if (LlegadoALaCesta[0]&& LlegadoALaCesta[1]&& LlegadoALaCesta[2])
            {
                Debug.Log("Dentro de la cesta");
                dentroDeLaCesda = true;

                //Iniciar corroutina para bajar el gancho y soltar el premio
                if (dentroDeLaCesda) {
                    StartCoroutine(SoltarPremioEnLaCesta(1.5f));
                    dentroDeLaCesda = false;
                }
            }


           


        }


        if (bajarGanchoYSoltarPremio)
        {
            if (AlturaGancho.position.y > alturaMinima)
            {

                Debug.Log("Bajando");
                transform.Translate(0, speed_Claw * -1 * Time.deltaTime, 0);


            }
            else {

                StartCoroutine(AbrirClawEnlaCesta(1.0f));
                bajarGanchoYSoltarPremio = false;
            }
        }



        if (subirGanchoDeLaCesta)
        {
            if (AlturaGancho.position.y <= alturaMaxima)
            {


                transform.Translate(0, speed_Claw * Time.deltaTime, 0);


            }
            else
            {
                PuedeControlarse = true;
                subirGanchoDeLaCesta = false;

            }
        }




        if (PuedeControlarse)
        {

            //Movimentos Laterales

            if (transform.position.x > limitadorLeft.transform.position.x)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    
                    transform.Translate(speed * -1 * Time.deltaTime, 0, 0);
                   
                }
            }


            if (transform.position.x < limitadorRight.transform.position.x)
            {
                if (Input.GetKey(KeyCode.D))
                {
                   
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                   
                }
            }

            //Movimientos Forward

            if (transform.position.z < limitadorBack.transform.position.z)
            {
                if (Input.GetKey(KeyCode.W))
                {
                   
                    transform.Translate(0, 0, speed * Time.deltaTime);
                   
                }
            }


            if (transform.position.z > limitadorFront.transform.position.z)
            {
                if (Input.GetKey(KeyCode.S))
                {
                   
                    transform.Translate(0, 0, speed * -1 * Time.deltaTime);
                  
                }
            }

            //Bajar Gancho

            if (AlturaGancho.position.y > alturaMinima)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    
                    transform.Translate(0, speed_Claw * -1 * Time.deltaTime, 0);
                   
                }
            }
            else
            {

                StartCoroutine(CerrarClaw(2.0f));
                PuedeControlarse = false;
                




            }

        }





    }

    //coroutina para recoger el premio

    IEnumerator CerrarClaw(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);


        CerrarClaw();


        yield return new WaitForSeconds(waitTime);


            Debug.Log("Subiendo gancho");
            SoltarPremio = true;
            
        
    }

    //coroutina para soltar el premio

    IEnumerator SoltarPremioEnLaCesta(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        bajarGanchoYSoltarPremio = true;
        SoltarPremio = false;
        Debug.Log("bajando gancho");


        yield return new WaitForSeconds(waitTime);

        
      


    }

    //coroutina para soltar el premio

    IEnumerator AbrirClawEnlaCesta(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        AbrirClaw();
        yield return new WaitForSeconds(waitTime);
        subirGanchoDeLaCesta = true;
        LlegadoALaCesta[0] = false;
        LlegadoALaCesta[1] = false;
        LlegadoALaCesta[2] = false;

    }

    public void AbrirClaw() {
        Debug.Log("Abriendo Gancho");
        animatorClaw.SetBool("Abrir", true);
        animatorClaw.SetBool("Cerrar", false);

    }



    public void CerrarClaw()
    {
        Debug.Log("Cerrar gancho");
        animatorClaw.SetBool("Abrir", false);
        animatorClaw.SetBool("Cerrar", true);

    }


}
