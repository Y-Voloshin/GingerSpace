namespace Catopus.Battle
{
    public class BattleResultModel
    {
        //public static BattleResultModel EmptyLose = new BattleResultModel { Victory = false, Reward = Reward.Empty }
        public bool Victory;
        public Reward Reward;
        public string Message;
    }
}