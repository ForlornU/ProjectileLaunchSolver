public interface ArcherInterface
{
    LaunchData Calculate(TargetData targetData);
    void Launch(LaunchData data);

}
