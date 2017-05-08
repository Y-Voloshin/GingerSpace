namespace Catopus.Battle
{
    public class BattleResultModel
    {
        /* Why LastBattleResult?
         * 
         * 1. No new objects for every battle
         * 
         * 2. No different functions for different results: 
         * You can see result easily from anywhere and do relative logic.
         * 
         * For example, Result form. There are four cases: planetwin, planetlose, shipwin, shiplose. It can be ore further
         * So, to tell game manager what to do next  you need four functions called by "OK" button.
         * BattleResult OK button has 4 meanings, calls 4 different functions in UIController, which makes 4 callbacks to GameManager, which has 4 logics to do.
         * We remove all this and just tell GameManager "We are done with BattleResultForm", and GameMAnager decides what to do next
         * */

        /// <summary>
        /// Last battle result is stored here
        /// </summary>
        public static BattleResultModel LastBattleResult = new BattleResultModel();

        //We do properties and protected constructor to deny manual instance creating to force leading way described up
        public BattlefieldType BattlefieldType { get; protected set; }
        public bool Victory { get; protected set; }
        public Reward Reward { get; protected set; }
        public string Message { get; protected set; }

        protected BattleResultModel() { }

        public static void UpdateLastResult(BattlefieldType BattlefieldType, bool Victory, Reward Reward, string Message)
        {
            LastBattleResult.BattlefieldType = BattlefieldType;
            LastBattleResult.Victory = Victory;
            LastBattleResult.Reward = Reward;
            LastBattleResult.Message = Message;

        }
    }
}