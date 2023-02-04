using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : Singleton<DataPersistenceManager>
{
    private string dataPath;
    private string configPath;
    private const string extension = ".xml";
    public ConfigData configuration;
    public Action<ConfigData> OnConfig;
    public Queue RequestConfig = new Queue();
    protected override void Awake()
    {
        base.Awake();
        dataPath = Application.dataPath;
        configPath = dataPath + "/config" + extension;
        configuration = ConfigData.Load(configPath);
        if(configuration == null)
        {
            configuration = new ConfigData();
            ConfigData.Save(configPath, configuration);
            SaveConfig();
            print("Create new config");
        }
        else
        {
            print("Loaded config");
        }
    }

    private void LateUpdate()
    {
        if(RequestConfig.Count > 0)
        {
            if (configuration == null) return;
            OnConfig?.Invoke(configuration);
            RequestConfig.Clear();
        }
    }

    public void SaveConfig()
    {
        ConfigData.Save(configPath, configuration);
    }

    public void RequestData()
    {
        if(configuration == null)
        {
            RequestConfig.Enqueue(null);
        }
        OnConfig?.Invoke(configuration);
    }
}
