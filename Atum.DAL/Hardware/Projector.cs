using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class Projector
    {
        public int Id { get; set; }
        public List<int> LightFieldCalibration { get; set; }

        public Projector()
        {

        }

        public Projector(int id)
        {
            this.LightFieldCalibration = new List<int>();
            this.LightFieldCalibration.AddRange(new[]{
                0,0,0,0,100,150,200,200,
                0,0,0,0,0,100,150,200,
                0,0,100,0,0,100,100,150,
                0,100,100,100,0,0,0,100
            });
        }
    }

    [Serializable]
    public class Projectors : List<Projector>
    {

    }
}
