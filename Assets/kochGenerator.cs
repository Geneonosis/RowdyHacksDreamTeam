using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kochGenerator : MonoBehaviour
{
    protected enum _axis
    {
        XAxis,
        YAxis,
        ZAxis
    };
    [SerializeField]
    protected _axis axis = new _axis();
   


    //select what kind of shape you want
    protected enum _initiator
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon
    };

    public struct LineSegment
    {
        public Vector3 startpos { get; set; }
        public Vector3 endpos { get; set; }
        public Vector3 direction { get; set; }
        public float length { get; set; }
    };

    [SerializeField]
    protected AnimationCurve _generator;
    [SerializeField]
    protected _initiator initiator = new _initiator();
    protected Keyframe[] _keys;

    [System.Serializable]
    public struct StartGen
    {
        public bool outwards;
        public float scale;
    };

    public StartGen[] startGen;

    [SerializeField]
    protected bool _useBezierCurves;
    [SerializeField]
    [Range(8,24)]
    protected int _bezierVertexCount;

    protected int _generationNum;

    protected int _initiatorVertexAmount;
    private Vector3[] _initiatorVertex;
    private Vector3 _rotateVector;
    private Vector3 _rotateAxis;
    private float _initialRotation;
    [SerializeField]
    protected float _initiatorSize;
    protected Vector3[] _position;
    protected Vector3[] _targetPosition;
    protected Vector3[] _bezierPosition;
    private List<LineSegment> _lineSegment;

    protected Vector3[] BezierCurve(Vector3[] points, int vertexCount)
    {
        var pointList = new List<Vector3>();
        for (int i=0; i<points.Length ; i+=2)
        {
            //checks if there are enough points left to create curve
            if (i+2 <= points.Length - 1)
            {
                for (float ratio = 0f; ratio <= 1; ratio += 1.0f / vertexCount)
                {
                    var tangentLineVertex1 = Vector3.Lerp(points[i], points[i+1], ratio);
                    var tangentLineVertex2 = Vector3.Lerp(points[i+1], points[i+2], ratio);
                    var bezierpoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
                    pointList.Add(bezierpoint);
                }

            }

        }
        return pointList.ToArray();
    }


    private void Awake()
    {
        GetInitiatorVertices();
        //assign lists & arrays
        _position = new Vector3[_initiatorVertexAmount+1];
        _targetPosition = new Vector3[_initiatorVertexAmount + 1];
        _lineSegment = new List<LineSegment>();
        _keys = _generator.keys;

        _rotateVector = Quaternion.AngleAxis(_initialRotation, _rotateAxis) * _rotateVector;
        for (int i = 0; i < _initiatorVertexAmount; i++)
        {
            _position[i] = _rotateVector * _initiatorSize;
            _rotateVector = Quaternion.AngleAxis(360 / _initiatorVertexAmount, _rotateAxis) * _rotateVector;

        }
        _position[_initiatorVertexAmount] = _position[0];
        _targetPosition = _position;

        for (int i=0; i<startGen.Length; i++)
        {
            KochGenerate(_targetPosition, startGen[i].outwards, startGen[i].scale);

        }

    }

    protected void KochGenerate(Vector3[] positions, bool outwards, float genMultiplier)
    {
        _lineSegment.Clear();

        //creating Line Segments
        for (int i = 0; i < positions.Length - 1; i++)
        {
            LineSegment temp = new LineSegment();
            temp.startpos = positions[i];
            if (i == positions.Length - 1)
            {
                temp.endpos = positions[0];
            }
            else
            {
                temp.endpos = positions[i + 1];
            }
            temp.direction = (temp.endpos - temp.startpos).normalized;
            temp.length = Vector3.Distance(temp.endpos, temp.startpos);
            _lineSegment.Add(temp);
        }

        //add line segment points to point array

        List<Vector3> newPos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();

        for (int i = 0; i < _lineSegment.Count; i++)
        {
            newPos.Add(_lineSegment[i].startpos);
            targetPos.Add(_lineSegment[i].startpos);


            for (int j = 0; j < _keys.Length - 1; j++)
            {
                float moveAmount = _lineSegment[i].length * _keys[j].time;
                float heightAmount = (_lineSegment[i].length * _keys[j].value) * genMultiplier;
                Vector3 resultPos = _lineSegment[i].startpos + (_lineSegment[i].direction * moveAmount);
                Vector3 Dir;
                if (outwards == true)
                {
                    Dir = Quaternion.AngleAxis(-90, _rotateAxis) * _lineSegment[i].direction;
                }
                else
                {
                    Dir = Quaternion.AngleAxis(90, _rotateAxis) * _lineSegment[i].direction;
                }
                newPos.Add(resultPos);
                targetPos.Add(resultPos + (Dir * heightAmount));

            }
        }
        newPos.Add(_lineSegment[0].startpos);
        targetPos.Add(_lineSegment[0].startpos);
        _position = new Vector3[newPos.Count];
        _targetPosition = new Vector3[targetPos.Count];
        _position = newPos.ToArray();
        _targetPosition = targetPos.ToArray();
        _bezierPosition = BezierCurve(_targetPosition, _bezierVertexCount);


        _generationNum++;
    }

    public float lengthOfSides;
    private void OnDrawGizmos()
    {
        GetInitiatorVertices();
        _initiatorVertex = new Vector3[_initiatorVertexAmount];
        _rotateVector = Quaternion.AngleAxis(_initialRotation, _rotateAxis) * _rotateVector;

        for (int i=0; i<_initiatorVertexAmount; i++)
        {
            _initiatorVertex[i] = _rotateVector * _initiatorSize;
            _rotateVector = Quaternion.AngleAxis(360 / _initiatorVertexAmount, _rotateAxis) * _rotateVector;

        }
        for (int i=0; i<_initiatorVertexAmount; i++)
        {
            Gizmos.color = Color.white;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.matrix = rotationMatrix; 
            if (i< _initiatorVertexAmount-1)
            {
                Gizmos.DrawLine(_initiatorVertex[i], _initiatorVertex[i + 1]);
            }
            else
            {
                Gizmos.DrawLine(_initiatorVertex[i], _initiatorVertex[0]);
            }
        }

        lengthOfSides = Vector3.Distance(_initiatorVertex[0], _initiatorVertex[1]) * 0.5f;
    }
    private void GetInitiatorVertices()
    {
        switch (initiator)
        {
            case _initiator.Triangle:
                _initiatorVertexAmount = 3;
                _initialRotation = 0; //(36s0/vertexAmnt) / 2
                break;
            case _initiator.Square:
                _initiatorVertexAmount = 4;
                _initialRotation = 45;
                break;
            case _initiator.Pentagon:
                _initiatorVertexAmount = 5;
                _initialRotation = 36;
                break;
            case _initiator.Hexagon:
                _initiatorVertexAmount = 6;
                _initialRotation = 30;
                break;
            case _initiator.Heptagon:
                _initiatorVertexAmount = 7;
                _initialRotation = 25.71428f;
                break;
            case _initiator.Octagon:
                _initiatorVertexAmount = 8;
                _initialRotation = 22.5f;
                break;
            default:
                _initiatorVertexAmount = 3;
                _initialRotation = 0;
                break;
        }

        switch (axis)
        {
            case _axis.XAxis:
                _rotateVector = new Vector3(1, 0, 0);
                _rotateAxis = new Vector3(0, 0, 1);
                break;
            case _axis.YAxis:
                _rotateVector = new Vector3(0, 1, 0);
                _rotateAxis = new Vector3(1, 0, 0);
                break;
            case _axis.ZAxis:
                _rotateVector = new Vector3(0, 0, 1);
                _rotateAxis = new Vector3(0, 1, 0);
                break;
            default:
                _rotateVector = new Vector3(0, 1, 0);
                _rotateAxis = new Vector3(1, 0, 0);
                break;

        }

    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
