namespace RealFriendlyFarm.Ecs.Component;

public record struct RotatorComponent(
    float VelocityX, 
    float VelocityY, 
    float VelocityZ
);