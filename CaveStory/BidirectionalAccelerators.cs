using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public class BidirectionalAccelerators
    {
        public ConstantAccelerator positive;
        public ConstantAccelerator negative;

        public BidirectionalAccelerators(AccelerationUnit acceleration, VelocityUnit maxVelocity)
        {
            positive = new ConstantAccelerator(acceleration, maxVelocity);
            negative = new ConstantAccelerator(-acceleration, -maxVelocity);
        }
    }
}
