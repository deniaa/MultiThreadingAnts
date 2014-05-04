using System.Drawing;

namespace AntsBattle
{
    public interface IWorld
    {
        Size Size { get; }
        IAntAI WhiteAntAI { get; }
    }
}