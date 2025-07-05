using Godot;
using LabyrinthExplorer3D.scripts.game.behaviours;
using LabyrinthExplorer3D.scripts.game.player;

namespace LabyrinthExplorer3D.scripts.game.npc.behaviours;

[GlobalClass]
public partial class EarsNpcBehaviour3D : CharacterBehaviour3D
{
    [Export] public bool HearsPlayer;
    [Export] public float DistanceToPlayer;
    [Export] public double TimeWithAudio;
    [Export] public double TimeSinceLastAudio;
    
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        HearsPlayer = OwningPlayer.Ears.IsHearingAnyPlayer();
        
        if (HearsPlayer)
        {
            DistanceToPlayer = OwningPlayer.GlobalPosition.DistanceTo(OwningPlayer.Ears.PlayersInRange[0].GlobalPosition);
            TimeWithAudio += delta;
            TimeSinceLastAudio = 0;       
        }
        else
        {
            DistanceToPlayer = -1;
            TimeWithAudio = 0;       
            TimeSinceLastAudio += delta;       
        }
    }
}