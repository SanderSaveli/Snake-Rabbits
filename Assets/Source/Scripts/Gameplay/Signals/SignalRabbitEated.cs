namespace SanderSaveli.Snake
{
    public readonly struct SignalRabbitEated
    {
        public readonly Rabbit Rabbit;

        public SignalRabbitEated(Rabbit rabbit)
        {
            Rabbit = rabbit;
        }
    }
}
