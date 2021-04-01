using UnityEngine;

public class LaserTest2 : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private EffectableTargetBank _targetBank;
    private TowerComponentBeamData _data;
    public float startFireCounter;
    public float startFireCounterMax;
    public float laserDuration;
    public float laserDurationTimer;
    public float widthOscilationValue;
    [SerializeField] private (float, float) _widthMinMax;
    public float beamWidthBase;
    
    [SerializeField]
    private float beamWidth;

    public float BeamWidth
    {
        get => beamWidth;
        set
        {
            beamWidth = value;
            _lineRenderer.startWidth = beamWidth;
            _lineRenderer.endWidth = beamWidth;
        }
    }


    private bool _widthDirection;

    void OscilateBeamWidth()
    {
        if (BeamWidth <= _widthMinMax.Item1)
        {
            _widthDirection = true;
        }
        else if (BeamWidth >= _widthMinMax.Item2)
        {
            _widthDirection = false;
        }

        if (_widthDirection == true)
        {
            BeamWidth += Time.deltaTime * widthOscilationValue * laserDuration;
        }
        else
        {
            BeamWidth -= Time.deltaTime * widthOscilationValue * laserDuration;
        }
    }

    private bool _laserSwitch;

    public bool LaserSwitch
    {
        get => _laserSwitch;
        set
        {
            if (value != _laserSwitch)
            {
                if (value == true)
                {
                    _lineRenderer.enabled = true;
                }

                if (value == false)
                {
                    _lineRenderer.enabled = false;
                }
            }

            _laserSwitch = value;
        }
    }
    
    public void RenderBeam()
    {
        bool laserEnabled;
        
        
        if (laserDurationTimer > 0)
        { 
            LaserSwitch = true;
            _lineRenderer.SetPosition(0, exitPoint.transform.position);
        _lineRenderer.SetPosition(1,finalPoint.transform.position);
        laserDurationTimer -= Time.deltaTime;
        }

        if (laserDurationTimer <= 0)
        {
            LaserSwitch = false;
        }

        startFireCounter += Time.deltaTime;
        if (startFireCounter >= startFireCounterMax)
        {
            laserDurationTimer = laserDuration;
            startFireCounter = 0;
        }
    }
    
    
    [SerializeField]
    private Effectable Target;
    

    private ProjectileExitPoint exitPoint;
    private ProjectileFinalPoint finalPoint;


    private void Update()
    {
        RenderBeam();
        OscilateBeamWidth();
    }


    private void Awake()
    {
        
        _lineRenderer = GetComponent<LineRenderer>();
        _targetBank = GetComponent<EffectableTargetBank>();
    }

    private void Start()
    {
        _targetBank.onTargetAdd += delegate(Effectable effectable) { Target = effectable;};
        _targetBank.onTargetRemove += delegate(int s) {
            if (Target.GameObjectID == s)
            {
                Target = null;
            } };
        exitPoint = GetComponentInChildren<ProjectileExitPoint>();
        finalPoint = GetComponentInChildren<ProjectileFinalPoint>();
        BeamWidth = beamWidthBase;
        _widthMinMax.Item1 = BeamWidth - widthOscilationValue;
        _widthMinMax.Item2 = BeamWidth + widthOscilationValue;
    }
}


