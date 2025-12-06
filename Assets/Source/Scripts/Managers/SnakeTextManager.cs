using Newtonsoft.Json;
using SanderSaveli.UDK;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.Snake
{
    public class SnakeTextManager : TextsManager<Language, LanguageTextData>
    {
        protected const string _resourceKey = "Texts/text";
        protected IStorageService _storageService;

        private void Awake()
        {
            _storageService = new ResourceStorageService();
        }

        public override string GetCurrentLanguageValue(LanguageTextData texts)
        {
            switch (Language)
            {
                case Language.EN:
                    return texts.en;
                case Language.RU:
                    return texts.ru;
                default:
                    throw new Exception($"There is no case for Language: {Language}");
            }
        }

        protected override void GetTextFromFile(Action<string> callback)
        {
            _storageService.Load<TextAsset>(_resourceKey, asset =>
            {
                if (asset != null)
                    callback?.Invoke(asset.text);
                else
                    callback?.Invoke(null);
            });
        }

        protected override void GetTextFromServer(Action<string> callback)
        {
            StartCoroutine(APIServer.GET(Const.GET_TEXT, callback));
        }

        protected override Dictionary<string, LanguageTextData> ParseResponce(string responce)
        {
            Dictionary<string, LanguageTextData> textDatas = new Dictionary<string, LanguageTextData>();
            List<LanguageTextData> languageTexts = JsonConvert.DeserializeObject<List<LanguageTextData>>(responce);

            foreach (LanguageTextData textData in languageTexts)
            {
                textDatas.Add(textData.id, textData);
            }
            return textDatas;
        }

        protected override void SaveToFile(string data)
        {
            Debug.Log("Save to file");
            List<LanguageTextData> d = JsonConvert.DeserializeObject<List<LanguageTextData>>(data);
            _storageService.Save(_resourceKey, d);
        }
    }
}
