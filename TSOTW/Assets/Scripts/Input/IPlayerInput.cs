public interface IPlayerInput {
    bool any { get; }
    float xAxis { get; }
    float yAxis { get; }
    bool PrimaryActionDown { get; }
    bool SecondaryActionDown { get; }

    void Poll(float dt);
}
