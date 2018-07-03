using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common.plugins
{
	public static class DomainHelper
	{
		/// <summary>
		/// Создает домен приложения
		/// </summary>
		/// <param name="appPath">Директория основного приложения</param>
		/// <param name="pluginsPath">Директория для поиска сборок</param>
		/// <returns></returns>
		public static AppDomain CreateDomain(string appPath, string pluginsPath)
		{
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = appPath;
			setup.PrivateBinPath = pluginsPath;
			return AppDomain.CreateDomain("Temporary domain", null, setup);
		}

		/// <summary>
		/// Выгружает домен приложения.
		/// </summary>
		/// <param name="domain">Домен подлежащий выгрузке.</param>
		public static void UnloadDomain(AppDomain domain)
		{
			AppDomain.Unload(domain);
		}
	}
}
