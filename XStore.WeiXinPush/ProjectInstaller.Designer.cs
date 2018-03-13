namespace WeiXinPush
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceInstallerHomoryResourceCountService = new System.ServiceProcess.ServiceInstaller();
            this.WeiXinPushServicesserviceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            // 
            // serviceInstallerHomoryResourceCountService
            // 
            this.serviceInstallerHomoryResourceCountService.Description = "订单统计推送";
            this.serviceInstallerHomoryResourceCountService.DisplayName = "OrderWeiXinPush";
            this.serviceInstallerHomoryResourceCountService.ServiceName = "OrderWeiXinPush";
            this.serviceInstallerHomoryResourceCountService.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // WeiXinPushServicesserviceProcessInstaller
            // 
            this.WeiXinPushServicesserviceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.WeiXinPushServicesserviceProcessInstaller.Password = null;
            this.WeiXinPushServicesserviceProcessInstaller.Username = null;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceInstallerHomoryResourceCountService,
            this.WeiXinPushServicesserviceProcessInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceInstaller serviceInstallerHomoryResourceCountService;
        private System.ServiceProcess.ServiceProcessInstaller WeiXinPushServicesserviceProcessInstaller;
    }
}