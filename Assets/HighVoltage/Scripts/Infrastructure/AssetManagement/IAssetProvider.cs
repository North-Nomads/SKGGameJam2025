﻿using UnityEngine;
using HighVoltage.Infrastructure.Services;

namespace HighVoltage.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        public GameObject Instantiate(string path);
        public GameObject Instantiate(string path, Vector3 at);
        public GameObject Instantiate(string path, Transform parent);
        public T Instantiate<T>(string path) where T : MonoBehaviour;
        public T Instantiate<T>(string path, Vector3 at) where T : MonoBehaviour;
        public T Instantiate<T>(string path, Transform parent) where T : MonoBehaviour;
        public T Instantiate<T>(T prefab, Vector3 position) where T : MonoBehaviour;
    }
}