using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Xml.Linq; 

namespace CryEngine
{
    class Transform
    {
        Translation _translation;
        Rotation _rotation;
        Scale _scale;

        public Transform()
        {
            _translation = new Translation();
            _rotation = new Rotation();
            _scale = new Scale();
        }
        public Transform(Vector3 pos, Vector3 rot, Vector3 scale)
        {
            _translation = new Translation(pos);
            _rotation = new Rotation(rot);
            _scale = new Scale(scale);
        }

        public XElement Get()
        {
            XElement transform = new XElement("Transform");

            transform.Add(_translation.Get());
            transform.Add(_rotation.Get());
            transform.Add(_scale.Get());

            return transform;
        }

        //private classes
        private class Position
        {
            Vector3 _position;
            public Position()
            {
                _position = new Vector3(0, 0, 0);
            }
            public Position(Vector3 pos)
            {
                _position = pos;
            }
            public XElement Get()
            {
                XElement position = new XElement("position");

                XElement element = new XElement("element"); 
                element.Add(new XAttribute("element", Utilities.FormatFloat(_position.x)));
                position.Add(element);

                element = new XElement("element");
                element.Add(new XAttribute("element", Utilities.FormatFloat(_position.y)));
                position.Add(element);

                element = new XElement("element");
                element.Add(new XAttribute("element", Utilities.FormatFloat(_position.z)));
                position.Add(element);

                return position;
            }
        }
        private class Rotation
        {
            Angles3 _rotation;

            public Rotation()
            {
                _rotation = new Vector3(0,0,0);
            }
            public Rotation(Vector3 rot)
            {
                _rotation = rot;
            }

            public XElement Get()
            {
                XElement rotation = new XElement("rotation");

                rotation.Add(new XAttribute("x", Utilities.FormatFloat(_rotation.x)));
                rotation.Add(new XAttribute("y", Utilities.FormatFloat(_rotation.y)));
                rotation.Add(new XAttribute("z", Utilities.FormatFloat(_rotation.z)));

                return rotation;
            }

        }
        private class Scale
        {
            Vector3 _scale;

            public Scale()
            {
                _scale = new Vector3(1,1,1);
            }
            public Scale(Vector3 scale)
            {
                _scale = scale;
            }
            public XElement Get()
            {
                XElement scale = new XElement("scale");

                XElement element = new XElement("element");
                element.Add(new XAttribute("element", Utilities.FormatFloat(_scale.x)));
                scale.Add(element);

                element = new XElement("element");
                element.Add(new XAttribute("element", Utilities.FormatFloat(_scale.y)));
                scale.Add(element);

                element = new XElement("element");
                element.Add(new XAttribute("element", Utilities.FormatFloat(_scale.z)));
                scale.Add(element);

                return scale;
            }
        }
        private class Translation
        {
            Position _position; 

            public Translation()
            {
                _position = new Position();
            }
            public Translation(Vector3 pos)
            {
                _position = new Position(pos);
            }

            public XElement Get()
            {
                XElement translation = new XElement("translation");

                XElement postition = _position.Get();
                translation.Add(postition);

                return translation;
            }
        }
        private class Element
        {

        }
        
    }
    
}
