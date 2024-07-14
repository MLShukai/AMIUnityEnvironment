# Unity AMI Environment

自律機械知能 (AMI)をUnity ML Agents環境で実験するための Unity プロジェクト。

## 導入方法

### OS

* MacOS
* Linux (2024/07/14 未検証)
* Windows (2024/07/14 未検証。動作するはず。)

### Unity

[Unity Hub](https://unity.com/ja/download) をインストールし、次のバージョンのUnityをインストールする。

* Version: 6000.0.5f1

実行するOSのプラットフォームに合わせて `Build Support`も追加する。

### 動作テスト

1. Unityプロジェクトのビルド

   Unity エディタの `File` -> `Build Profiles`から、動作するOSプラットフォームに合わせて `Build`する。

2. Pythonと依存関係のインストール

   python3.10.1 - 3.10.12 のPythonをインストールし、`mlagents_envs`をインストールする。

   ```bash
   pip install mlagents-envs==1.0.0
   ```

3. 実行

   [`Assets/AMI/PythonSampleScript/unity_mlagents_gym_interaction.py`](Assets/AMI/PythonSampleScript/unity_mlagents_gym_interaction.py)を実行する。実行方法に関してはファイルに記載しているのでそこを確認。
