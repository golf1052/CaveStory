using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaveStory
{
    public interface Damageable
    {
        GameUnit CenterX { get; }
        GameUnit CenterY { get; }
        DamageText DamageText { get; }
    }
}
