using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour {

	//GENERAL VARS
	#region General Vars

	public Transform LightsPrefab;
	public Transform ParticlesFront;
	public Transform ParticlesBack;

	public Transform SC0001_GEOMETRY;
	public Transform SC0002_GEOMETRY;
	public Transform SC0003_GEOMETRY;
	public Transform SC0004_GEOMETRY;
	public Transform SC0005_GEOMETRY;

	#endregion

	//SC001 VARS
	#region SC001 Vars

	[Header(" - BEGIN SCENE 001 VARIABLES - ")]

	public float SC0001_Wait_Lights_ON = 1.0f;
	public float SC0001_Wait_BaseSoundTrk = 1.0f;
	public float SC0001_Wait_SubTrk = 1.0f;
	public float SC0001_Wait_BabyCryTrk = 1.0f;
	public float SC0001_Wait_Particles_ON = 1.0f;
	public float SC0001_Wait_PortalOpen = 1.0f;
	public float SC0001_Wait_PortalClose = 1.0f;
	public float SC0001_Wait_Void_Sound = 1.0f;
	public float SC0001_Wait_LightShaft_ON = 1.0f;
	public float SC0001_Wait_LightShaft_OFF = 1.0f;
	public float SC0001_Wait_To_SC0002_TRANS = 1.0f;
	private bool SC0001LightIntensityUP = false;
	private bool SC0001LightShaftIntensityUP = false;
	private bool SC0001LightShaftIntensityDown = false;
	private bool SC0001StartTransitionToSC0002 = false;
	public AudioSource BaseSoundtrack;
	public AudioSource SubTrack;
	public AudioSource BabyCry;
	public AudioSource PortalOpen;
	public AudioSource PortalClose;
	public AudioSource VoidSound;



	#endregion

	#region SC002 Vars

	[Header(" - BEGIN SCENE 002 VARIABLES - ")]

	public float SC0002_Wait_To_Init_Morph = 1.0f;
	public float SC0002_Wait_To_End_Morph = 1.0f;
	public AudioSource SC0002BaseSoundtrack;
	public AudioSource SC0002SubTrack;
	public float SC0002_Wait_To_SC0003_TRANS = 1.0f;
	private bool SC0002StartTransitionToSC0003 = false;
	private bool fade_0002_sound = false;

	#endregion

	#region SC003 Vars

	[Header(" - BEGIN SCENE 003 VARIABLES - ")]

	public float SC0003_Wait_PortalOpen = 1.0f;
	public float SC0003_Wait_PortalClose = 1.0f;
	public AudioSource SC0003SubTrack;
	public AudioSource SC0003MaleTrack;
	public AudioSource SC0003Femaletrack;
	public AudioSource SC0003PortalOpen;
	public AudioSource SC0003PortalClose;
	public float SC0003_Wait_To_SC0004_TRANS = 1.0f;
	private bool SC0003StartTransitionToSC0004 = false;
	//private bool fade_0003_sound = false;

	#endregion

	#region SC004 Vars

	[Header(" - BEGIN SCENE 004 VARIABLES - ")]

	public AudioSource SC0004BaseSoundtrack;
	public AudioSource SC0004SubTrack;
	public float SC0004_Wait_To_SC0005_TRANS = 1.0f;
	//private bool SC0004StartTransitionToSC0005 = false;
	//private bool fade_0003_sound = false;

	#endregion

	// INIT
	void Start () {

		DisableNonRelevantAssets();
		TurnLightsOff();
		SC0001TurnLightShaftOff();
		ClearAndStopParticles();

		#region COROUTINE CALLS
		StartCoroutine(SC0001_WaitLights());
		StartCoroutine(SC0001_WaitLightShaftOn());
		StartCoroutine(SC0001_WaitLightShaftOff());
		StartCoroutine(SC0001_WaitBaseSound());
		StartCoroutine(SC0001_WaitSubSound());
		StartCoroutine(SC0001_WaitBabyCrySound());
		StartCoroutine(SC0001_WaitPortalOpenSound());
		StartCoroutine(SC0001_WaitPortalCloseSound());
		StartCoroutine(SC0001_WaitVoidSound());
		StartCoroutine(WaitParticlesOn());
		StartCoroutine(StartScene0002());
		StartCoroutine(SC0002SoundFade());
		StartCoroutine(StartScene0003());
		StartCoroutine(SC0003_WaitPortalOpenSound());
		StartCoroutine(SC0003_WaitPortalCloseSound());
		StartCoroutine(StartScene0004());
		#endregion
	}

	// EACH FRAME
	void Update () {
		MaxUpLights();
		SC0001MaxUpLightShaft();
		SC0001MinDownLightShaft();
		TransitionToSC0002();
		FadeSC0002Sound();
		TransitionToSC0003();
		TransitionToSC0004();
	}

	#region --------------------------------> COROUTINES
	IEnumerator SC0001_WaitLights(){
		yield return new WaitForSeconds(SC0001_Wait_Lights_ON);
		TurnLightsOn();
		TurnSpecOfLightOff();
	}

	IEnumerator SC0001_WaitLightShaftOn(){
		yield return new WaitForSeconds(SC0001_Wait_LightShaft_ON);
		LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().enabled = true;
		LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().m_Brightness = 0.0f;
		SC0001LightShaftIntensityUP = true;
	}

	IEnumerator SC0001_WaitLightShaftOff(){
		yield return new WaitForSeconds(SC0001_Wait_LightShaft_OFF);
		SC0001LightShaftIntensityDown = true;
	}

	IEnumerator SC0001_WaitBaseSound(){
		yield return new WaitForSeconds(SC0001_Wait_BaseSoundTrk);
		BaseSoundtrack.Play();
	}

	IEnumerator SC0001_WaitSubSound(){
		yield return new WaitForSeconds(SC0001_Wait_SubTrk);
		SubTrack.Play();
	}

	IEnumerator SC0001_WaitBabyCrySound(){
		yield return new WaitForSeconds(SC0001_Wait_BabyCryTrk);
		BabyCry.Play();
	}

	IEnumerator SC0001_WaitPortalOpenSound(){
		yield return new WaitForSeconds(SC0001_Wait_PortalOpen);
		PortalOpen.Play();
	}

	IEnumerator SC0001_WaitPortalCloseSound(){
		yield return new WaitForSeconds(SC0001_Wait_PortalClose);
		PortalClose.Play();
	}

	IEnumerator SC0001_WaitVoidSound(){
		yield return new WaitForSeconds(SC0001_Wait_Void_Sound);
		VoidSound.Play();
	}

	IEnumerator WaitParticlesOn(){
		
		yield return new WaitForSeconds(SC0001_Wait_Particles_ON);
		ParticlesFront.gameObject.GetComponent<ParticleSystem>().Play();
		ParticlesBack.gameObject.GetComponent<ParticleSystem>().Play();
	}

	IEnumerator StartScene0002(){
		yield return new WaitForSeconds(SC0001_Wait_To_SC0002_TRANS);
		SC0001StartTransitionToSC0002 = true;
	}

	IEnumerator SC0002_WaitInitMorph(){
		yield return new WaitForSeconds(SC0002_Wait_To_Init_Morph);
		SC0002_GEOMETRY.GetChild(1).GetChild(0).gameObject.SetActive(false);
		SC0002_GEOMETRY.GetChild(2).GetChild(0).gameObject.SetActive(true);
	}
	
	IEnumerator SC0002_WaitEndMorph(){
		yield return new WaitForSeconds(SC0002_Wait_To_End_Morph);
		SC0002_GEOMETRY.GetChild(2).GetChild(0).gameObject.SetActive(false);
		SC0002_GEOMETRY.GetChild(1).GetChild(1).gameObject.SetActive(true);
	}

	IEnumerator StartScene0003(){
		yield return new WaitForSeconds(SC0002_Wait_To_SC0003_TRANS);
		SC0002StartTransitionToSC0003 = true;
	}

	IEnumerator SC0002SoundFade(){
		yield return new WaitForSeconds(SC0002_Wait_To_SC0003_TRANS - 4);
		fade_0002_sound = true;
	}

	IEnumerator SC0003_WaitPortalOpenSound(){
		yield return new WaitForSeconds(SC0003_Wait_PortalOpen);
		SC0003PortalOpen.Play();
	}

	IEnumerator SC0003_WaitPortalCloseSound(){
		yield return new WaitForSeconds(SC0003_Wait_PortalClose);
		SC0003PortalClose.Play();
	}

	IEnumerator StartScene0004(){
		yield return new WaitForSeconds(SC0003_Wait_To_SC0004_TRANS);
		SC0003StartTransitionToSC0004 = true;
	}

	#endregion

	#region --------------------------------> LIGHTNING MANAGEMENT METHODS

	//Turns cube Directional lights OFF
	public void TurnLightsOff(){
		//Top Light
		LightsPrefab.GetChild(0).gameObject.GetComponent<Light>().enabled = false;
		//Right Light
		LightsPrefab.GetChild(1).gameObject.GetComponent<Light>().enabled = false;
		//Left Light
		LightsPrefab.GetChild(2).gameObject.GetComponent<Light>().enabled = false;
		//Back Light
		LightsPrefab.GetChild(3).gameObject.GetComponent<Light>().enabled = false;
		//Front Light
		LightsPrefab.GetChild(4).gameObject.GetComponent<Light>().enabled = false;
		//Bottom Light
		LightsPrefab.GetChild(5).gameObject.GetComponent<Light>().enabled = false;

	}

	//Turns cube Directional Lights ON - 0 light intensity - triggers intensity up flag
	public void TurnLightsOn(){
		SetZeroLightIntensity();
		//Top Light
		LightsPrefab.GetChild(0).gameObject.GetComponent<Light>().enabled = true;
		//Right Light
		LightsPrefab.GetChild(1).gameObject.GetComponent<Light>().enabled = true;
		//Left Light
		LightsPrefab.GetChild(2).gameObject.GetComponent<Light>().enabled = true;
		//Back Light
		LightsPrefab.GetChild(3).gameObject.GetComponent<Light>().enabled = true;
		//Front Light
		LightsPrefab.GetChild(4).gameObject.GetComponent<Light>().enabled = true;
		//Bottom Light
		LightsPrefab.GetChild(5).gameObject.GetComponent<Light>().enabled = true;
		SC0001LightIntensityUP = true;
	}

	//Establishes cube Directional Lights at 0 light intensity (aux method) 
	public void SetZeroLightIntensity(){
		//Top Light
		LightsPrefab.GetChild(0).gameObject.GetComponent<Light>().intensity = 0.0f;
		//Right Light
		LightsPrefab.GetChild(1).gameObject.GetComponent<Light>().intensity = 0.0f;
		//Left Light
		LightsPrefab.GetChild(2).gameObject.GetComponent<Light>().intensity = 0.0f;
		//Back Light
		LightsPrefab.GetChild(3).gameObject.GetComponent<Light>().intensity = 0.0f;
		//Front Light
		LightsPrefab.GetChild(4).gameObject.GetComponent<Light>().intensity = 0.0f;
		//Bottom Light
		LightsPrefab.GetChild(5).gameObject.GetComponent<Light>().intensity = 0.0f;
	}

	//Establishes cube Directional lights at Max (0.96f) light intensity (aux method)
	public void SetMaxLightIntensity(){
		//Top Light
		LightsPrefab.GetChild(0).gameObject.GetComponent<Light>().intensity = 0.96f;
		//Right Light
		LightsPrefab.GetChild(1).gameObject.GetComponent<Light>().intensity = 0.96f;
		//Left Light
		LightsPrefab.GetChild(2).gameObject.GetComponent<Light>().intensity = 0.96f;
		//Back Light
		LightsPrefab.GetChild(3).gameObject.GetComponent<Light>().intensity = 0.96f;
		//Front Light
		LightsPrefab.GetChild(4).gameObject.GetComponent<Light>().intensity = 0.96f;
		//Bottom Light
		LightsPrefab.GetChild(5).gameObject.GetComponent<Light>().intensity = 0.96f;
	}

	// UPDATER Type -> Actually executes the light intensity increase from 0 to 0.96f
	public void MaxUpLights(){

		if(SC0001LightIntensityUP){
			for (int i=0;i<6;i++){
				LightsPrefab.GetChild(i).gameObject.GetComponent<Light>().intensity += 0.01f;
				if(LightsPrefab.GetChild(0).gameObject.GetComponent<Light>().intensity > 0.96f){
					SC0001LightIntensityUP = false;
					SetMaxLightIntensity();
					break;
				}
			}
		}

	}

	// UPDATER Type -> Actually executes the light shaft brightness increase from 0 to 0.8f
	public void SC0001MaxUpLightShaft(){
		if(LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().m_Brightness <= 0.8f && SC0001LightShaftIntensityUP){
			LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().m_Brightness += 0.01f;
			
		}
		else if(LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().m_Brightness > 0.8f && SC0001LightShaftIntensityUP)
		{
			SC0001LightShaftIntensityUP = false;
			
		}
	}

	// UPDATER Type -> Actually executes the light shaft brightness decrease from 0.8f to 0
	public void SC0001MinDownLightShaft(){
		if(LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().m_Brightness >= 0.0f && SC0001LightShaftIntensityDown){
			LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().m_Brightness -= 0.0001f;
			
		}
		else if(LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().m_Brightness <= 0 && SC0001LightShaftIntensityDown)
		{
			SC0001LightShaftIntensityDown = false;
			LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().enabled = false;
			
		}
	}

	//Turns Off the Spec Of Light at the top face of the cube
	public void TurnSpecOfLightOff(){
		LightsPrefab.GetChild(6).gameObject.GetComponent<Light>().enabled = false;
		LightsPrefab.GetChild(6).gameObject.GetComponent<LightShafts>().enabled = false;
	}

	//Turn OFF Top Light Shaft from Scene001 
	public void SC0001TurnLightShaftOff(){
		LightsPrefab.GetChild(0).gameObject.GetComponent<LightShafts>().enabled = false;
	}

	#endregion

	#region --------------------------------> PARTICLE SYSTEM MANAGEMENT METHODS

	public void ClearAndStopParticles(){
		ParticlesFront.gameObject.GetComponent<ParticleSystem>().Clear();
		ParticlesFront.gameObject.GetComponent<ParticleSystem>().Stop();
		ParticlesBack.gameObject.GetComponent<ParticleSystem>().Clear();
		ParticlesBack.gameObject.GetComponent<ParticleSystem>().Stop();
	}

	#endregion

	public void TransitionToSC0002(){
		if (SC0001StartTransitionToSC0002)
		{
			ClearSC0001();
			LoadSC0002();
			SC0001StartTransitionToSC0002 = false;
		}
	}

	public void ClearSC0001(){
		//BABY
		SC0001_GEOMETRY.GetChild(0).gameObject.SetActive(false);
		Destroy(SC0001_GEOMETRY.GetChild(0).gameObject);

		//CUBE SC0001 AND CUBE SC0002 TRANSITION
		SC0002_GEOMETRY.GetChild(0).gameObject.SetActive(true);
		SC0001_GEOMETRY.GetChild(1).gameObject.SetActive(false);
		Destroy(SC0001_GEOMETRY.GetChild(1).gameObject);

		//PLACENTA
		SC0001_GEOMETRY.GetChild(2).gameObject.SetActive(false);
		Destroy(SC0001_GEOMETRY.GetChild(2).gameObject);

		//UMBILICAL CORD
		SC0001_GEOMETRY.GetChild(3).gameObject.SetActive(false);
		Destroy(SC0001_GEOMETRY.GetChild(3).gameObject);
	}

	public void LoadSC0002(){
		// CUBE SC0002
		SC0002_GEOMETRY.GetChild(0).gameObject.SetActive(true);

		//(OLD) INIT BOY
		SC0002_GEOMETRY.GetChild(1).gameObject.SetActive(true);

		//(OLD) INIT TEEN
		SC0002_GEOMETRY.GetChild(2).gameObject.SetActive(true);

		//(OLD) INIT BOY - Boy_REF:Boy
		SC0002_GEOMETRY.GetChild(1).GetChild(0).gameObject.SetActive(true);
		//(OLD) INIT BOY - MaleTeenage_REF:MaleTeenage
		SC0002_GEOMETRY.GetChild(1).GetChild(1).gameObject.SetActive(false);

		//(OLD) INIT TEEN - BoyMorph
		SC0002_GEOMETRY.GetChild(2).GetChild(0).gameObject.SetActive(false);
		//(OLD) INIT TEEN - MTeenMorph
		SC0002_GEOMETRY.GetChild(2).GetChild(1).gameObject.SetActive(false);

		SC0002BaseSoundtrack.Play();
		SC0002SubTrack.Play();

		// MORPH CONTROL						
		StartCoroutine(SC0002_WaitInitMorph());
		StartCoroutine(SC0002_WaitEndMorph());


	}

	public void DisableNonRelevantAssets(){
		//ASSETS FROM SCENE 2
		SC0002_GEOMETRY.GetChild(0).gameObject.SetActive(false);
		SC0002_GEOMETRY.GetChild(1).gameObject.SetActive(false);
		SC0002_GEOMETRY.GetChild(2).gameObject.SetActive(false);

		//ASSETS FROM SCENE 3
		SC0003_GEOMETRY.GetChild(0).gameObject.SetActive(false);
		SC0003_GEOMETRY.GetChild(1).gameObject.SetActive(false);
		SC0003_GEOMETRY.GetChild(2).gameObject.SetActive(false);

		SC0004_GEOMETRY.gameObject.SetActive(false);
		SC0005_GEOMETRY.gameObject.SetActive(false);
	}

	public void TransitionToSC0003(){
		if (SC0002StartTransitionToSC0003){
			ClearSC0002();
			LoadSC0003();
			SC0002StartTransitionToSC0003 = false;
		}
	}

	public void ClearSC0002(){

		//CUBE SC0002 and CUBE SC0003 Transition
		SC0002_GEOMETRY.GetChild(0).gameObject.SetActive(false);
		SC0003_GEOMETRY.GetChild(0).gameObject.SetActive(true);
		Destroy(SC0002_GEOMETRY.GetChild(0).gameObject);

		//MALE_GRP_V2
		SC0002_GEOMETRY.GetChild(1).gameObject.SetActive(false);
		Destroy(SC0002_GEOMETRY.GetChild(1).gameObject);

		//MORPH_GRP_V2
		SC0002_GEOMETRY.GetChild(2).gameObject.SetActive(false);
		Destroy(SC0002_GEOMETRY.GetChild(2).gameObject);
	}

	public void LoadSC0003(){
		SC0003_GEOMETRY.GetChild(0).gameObject.SetActive(true);
		SC0003_GEOMETRY.GetChild(1).gameObject.SetActive(true);
		SC0003_GEOMETRY.GetChild(2).gameObject.SetActive(true);
		Debug.Log("SCENE 03 ON AIR");
		//SC0003SubTrack.Play();
		//SC0003MaleTrack.Play();
		//SC0003Femaletrack.Play();
	}

	public void FadeSC0002Sound(){
		if(fade_0002_sound){
			if(SC0002BaseSoundtrack.volume > 0.1f){
				SC0002BaseSoundtrack.volume -= 0.001f;
				SC0002SubTrack.volume -= 0.001f;;
			}
			else{
				SC0002BaseSoundtrack.volume = 0.0f;
				SC0002SubTrack.volume -= 0.0f;
				fade_0002_sound = false;
			}
		}
	}

	public void TransitionToSC0004(){
		if (SC0003StartTransitionToSC0004){
			ClearSC0003();
			LoadSC0004();
			SC0003StartTransitionToSC0004 = false;
		}
	}

	public void ClearSC0003(){

		//CUBE SC0003 and CUBE SC0004 Transition
		SC0003_GEOMETRY.GetChild(0).gameObject.SetActive(false);
		SC0004_GEOMETRY.GetChild(0).gameObject.SetActive(true);
		Destroy(SC0003_GEOMETRY.GetChild(0).gameObject);

		//MALE_GRP_V2
		SC0003_GEOMETRY.GetChild(1).gameObject.SetActive(false);
		Destroy(SC0003_GEOMETRY.GetChild(1).gameObject);

		//MORPH_GRP_V2
		SC0003_GEOMETRY.GetChild(2).gameObject.SetActive(false);
		Destroy(SC0003_GEOMETRY.GetChild(2).gameObject);
	}

	public void LoadSC0004(){
		SC0004_GEOMETRY.GetChild(0).gameObject.SetActive(true);
		SC0004_GEOMETRY.GetChild(1).gameObject.SetActive(true);
		SC0004_GEOMETRY.GetChild(2).gameObject.SetActive(true);
		SC0004_GEOMETRY.GetChild(3).gameObject.SetActive(true);
	}
}
