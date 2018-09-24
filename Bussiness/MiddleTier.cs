namespace Bussiness
{
    public class MiddleTier
    {

	    #region Instance

	    private static MiddleTier _sMiddleTier;
	    private static readonly object SMiddleTierMutex = new object();

	    public static void EnsureSingletonLoaded()
	    {
		    if (_sMiddleTier == null)
		    {
			    lock (SMiddleTierMutex)
			    {
				    if (_sMiddleTier == null)
				    {
					    var mt = new MiddleTier();
					    _sMiddleTier = mt;
				    }
			    }
		    }
	    }

	    public static MiddleTier Instance
	    {
		    get
		    {
			    EnsureSingletonLoaded();
			    return _sMiddleTier;
		    }
	    }


		#endregion

		#region LogManager

		private static readonly object MutexLogmanager = new object();
	    private LogManager _logManager;
	    public LogManager LogManager
	    {
		    get
		    {
			    if (_logManager == null)
			    {
				    lock (MutexLogmanager)
				    {
					    if (_logManager == null)
					    {
						    _logManager = new LogManager(this);
					    }
				    }
			    }
			    return _logManager;
		    }
	    }


		#endregion
		
		#region MemberManager

		private static readonly object MutexMemberManager = new object();
	    private MemberManager _memberManager;
	    public MemberManager MemberManager
	    {
		    get
		    {
			    if (_memberManager == null)
			    {
				    lock (MutexMemberManager)
				    {
					    if (_memberManager == null)
					    {
						    _memberManager = new MemberManager(this);
					    }
				    }
			    }
			    return _memberManager;
		    }
	    }
		#endregion

		#region SystemManager


		private static readonly object MutexSystemManager = new object();
	    private SystemManager _systemManager;
	    public SystemManager SystemManager
	    {
		    get
		    {
			    if (_systemManager == null)
			    {
				    lock (MutexSystemManager)
				    {
					    if (_systemManager == null)
					    {
						    _systemManager = new SystemManager(this);
					    }
				    }
			    }
			    return _systemManager;
		    }
	    }

		#endregion

		#region ConfigManager

		private static readonly object MutexConfigManager = new object();
	    private static ConfigManager _configManager;
		public ConfigManager ConfigManager
		{
			get
			{
				lock (MutexConfigManager)
				{
					if (_configManager == null)
						_configManager = new ConfigManager(this);
				}
				return _configManager;
			}
		}

		#endregion

		#region BusinessConfig	

		private static readonly object MutexBusinessConfig = new object();
	    private static BusinessConfig _businessConfig;
	    public BusinessConfig BusinessConfig
	    {
		    get
		    {
			    lock (MutexBusinessConfig)
			    {
				    if (_businessConfig == null)
					    _businessConfig = ConfigManager.Read<BusinessConfig>();
			    }
			    return _businessConfig;
		    }
		    set => _businessConfig = value;
	    }

		#endregion
		
		#region TransferBillManager

		private static readonly object MutexTransferBillManager = new object();
	    private TransferBillManager _transferBillManager;
	    public TransferBillManager TransferBillManager
		{
		    get
		    {
			    if (_transferBillManager == null)
			    {
				    lock (MutexTransferBillManager)
				    {
					    if (_transferBillManager == null)
					    {
						    _transferBillManager = new TransferBillManager(this);
					    }
				    }
			    }
			    return _transferBillManager;
		    }
	    }

		#endregion

		#region MessageBoardManager

		private static readonly object MutexMessageBoardManager = new object();
	    private static MessageBoardManager _messageBoardManager;
	    public MessageBoardManager MessageBoardManager
		{
		    get
		    {
			    lock (MutexMessageBoardManager)
			    {
				    if (_messageBoardManager == null)
					    _messageBoardManager = new MessageBoardManager(this);
			    }
			    return _messageBoardManager;
		    }
	    }

		#endregion

		#region NoticeManager

		private static readonly object MutexNoticeManager = new object();
	    private static NoticeManager _noticeManager;
	    public NoticeManager NoticeManager
		{
		    get
		    {
			    lock (MutexNoticeManager)
			    {
				    if (_noticeManager == null)
					    _noticeManager = new NoticeManager(this);
			    }
			    return _noticeManager;
		    }
	    }

		#endregion

	    #region NoticeManager

	    private static readonly object MutexEquipmentManager = new object();
	    private static EquipmentManager _equipmentManager;
	    public EquipmentManager EquipmentManager
		{
		    get
		    {
			    lock (MutexEquipmentManager)
			    {
				    if (_equipmentManager == null)
					    _equipmentManager = new EquipmentManager(this);
			    }
			    return _equipmentManager;
		    }
	    }

	    #endregion
	}
}
