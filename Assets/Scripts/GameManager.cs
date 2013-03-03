using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	public GameObject m_Ship = null;
	public Vector3 m_ShipSpawnPosition = Vector3.zero;
	
	public Rect m_LivesRect;
	public Rect m_ScoreRect;
	
	public float m_WaveDuration = 10.0f;
	
	public TextMesh m_MessageText = null;
	bool m_IsAnimatingMessage = false;
	float m_TextAnimationTime = 2.0f;
	float m_TextAnimationStartTime = 0.0f;
	
	private int m_Score = 0;
	public int Score
	{
		get{return m_Score;}
		set{m_Score = value;}
	}
	
	private int m_Wave = 1;
	public int Wave
	{
		get{return m_Wave;}
	}
	
	private int m_Lives = 1;
	public int Lives
	{
		get{return m_Lives;}
	}
	
	private int m_MaxLives = 3;
	
	private bool m_IsPlaying = false;
	
	private float m_TimeUntilNextWave = 0.0f;
	
	private Player m_Player = null;
	
	private bool m_IsAnimatingEntry = false;
	private float m_EntryStartTime = 0.0f;
	private float m_EntryDuration = 3.0f;
	private Vector3 m_EntryStartPoint = Vector3.zero;
	
	void Start()
	{
		m_Player = m_Ship.GetComponent<Player>();
		m_Player.OnDying += OnPlayerDying;
	}
	
	public int GetDifficulty(int wave)
	{
		//TODO: Tweak actual difficulty curve not to be linear. Consider batching waves of non-linear curve
		return wave;
	}
	
	public void StartGame()
	{
		m_Ship.transform.localPosition = m_ShipSpawnPosition;
		m_Ship.GetComponent<Control>().enabled = true;
		m_Wave = 0;
		AnimateEntry();
	}
	
	void Update()
	{
		if(m_IsAnimatingEntry)
		{
			AnimateEntry();
		}
		
		if(m_IsPlaying)
		{
			UpdateWave();
		}
		
		if(m_IsAnimatingMessage)
		{
			AnimateText();
		}
		
		HandleInput();
	}
	
	void HandleInput()
	{
		if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		else if(Input.GetKeyDown(KeyCode.R))
		{
			m_IsPlaying = false;
			m_Lives = m_MaxLives;
			m_TimeUntilNextWave = 0.0f;
			StartGame();
		}
	}
	
	void UpdateWave()
	{
		m_TimeUntilNextWave -= Time.deltaTime;
		if(m_TimeUntilNextWave < 0.0f)
		{
			SpawnWave();
		}
	}
	
	void SpawnWave()
	{
		
		m_Wave++;
		ShowText("Wave " + m_Wave);
		m_TimeUntilNextWave = m_WaveDuration;
		
		//TODO: Select next wave from enemy types and difficulty budget
	}
	
	
	
	void OnGUI()
	{
		if(m_IsPlaying)
		{
			GUI.TextArea(m_LivesRect,"Lives: " + m_Lives);
			m_ScoreRect.width = 60 + 5*m_Score.ToString().Length;
			GUI.TextArea(m_ScoreRect, "Score: " + m_Score);
		}
	}
	
	public void OnPlayerDying()
	{
		m_Lives--;
		if(m_Lives == 0)
		{
			ShowText("GAME OVER	\nr to restart", false);
		}
		else
		{
			m_Player.Health = m_Player.MaxHealth;
			StartGame();
		}
	}
	
	public void AnimateEntry()
	{
		if(!m_IsAnimatingEntry)
		{
			m_EntryStartPoint = m_ShipSpawnPosition - new Vector3(0,50,0);
			m_IsAnimatingEntry = true;
			m_EntryStartTime = Time.time;
		}
		
		Vector3 entryPath = (m_ShipSpawnPosition - m_EntryStartPoint);
		
		if(Time.time - m_EntryStartTime > m_EntryDuration)
		{
			m_Ship.transform.localPosition = m_ShipSpawnPosition;
			m_IsAnimatingEntry = false;
			m_IsPlaying = true;
		}
		else
		{
			m_Ship.transform.localPosition = m_EntryStartPoint +  entryPath * ((Time.time - m_EntryStartTime)/m_EntryDuration);
		}
	}
	
	void ShowText(string text, bool fade=true)
	{
		m_MessageText.text = text;
		if(fade)
		{
			m_TextAnimationStartTime = Time.time;
			m_IsAnimatingMessage = true;
		}
		else
		{
			m_IsAnimatingMessage = false;
			m_MessageText.renderer.material.color = new Color(m_MessageText.renderer.material.color.r, 
				m_MessageText.renderer.material.color.g, 
				m_MessageText.renderer.material.color.b, 
				1);
		}
	}
	
	void HideText()
	{
		m_MessageText.text = "";
	}
		
	
	public void AnimateText()
	{
		if(Time.time - m_TextAnimationStartTime > m_TextAnimationTime)
		{
			m_MessageText.text = "";
			m_MessageText.renderer.material.color = new Color(m_MessageText.renderer.material.color.r, 
				m_MessageText.renderer.material.color.g, 
				m_MessageText.renderer.material.color.b, 
				1);
			m_IsAnimatingMessage = false;
		}
		else
		{
			m_MessageText.renderer.material.color = new Color(m_MessageText.renderer.material.color.r, 
				m_MessageText.renderer.material.color.g, 
				m_MessageText.renderer.material.color.b, 
				1.0f - (Time.time - m_TextAnimationStartTime)/m_TextAnimationTime);
		}
	}
}
