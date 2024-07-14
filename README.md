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

## 実装説明

`Assets/AMI/Scenes/SampleScene.unity`に最小限の環境を実装した。

### `VRChatDiscreteUser`プレハブ

DesktopモードでVRChatをプレイするユーザをエミュレートするものである。

Observationは `Camera Sensor`コンポーネントを用いて一人称視点で映像をキャプチャしている。

Actionは`Behavior Paramters`コンポーネントの`Actions`に記述されている。Unity 環境上へのアクションの実行については [`VRChatEmulatedAgent`コンポーネントのスクリプト](/Assets/AMI/Scripts/VRChatEmulatedAgent.cs) を読んでほしい。

`Decision Requester`の`Decision Period`は、Pythonに行動を要求する周期である。`Fixed Timestamp`(後述)が最小周期 (Period=1)であるため、指定したFPSで動作させる際は設定に注意が必要である。

### 参考資料

使用しているAPIについて詳しくは [Unity 公式の ML Agents Toolkitのドキュメンテーション](https://unity-technologies.github.io/ml-agents/)を参照。

[基本的な使い方は Toolkit Documentation](https://unity-technologies.github.io/ml-agents/ML-Agents-Toolkit-Documentation/)に記述されている。

その他特に読むと良いものとして、Python API Docsの[Low Level API (LLAPI) Documentation](https://unity-technologies.github.io/ml-agents/Python-LLAPI-Documentation/)と、[Gym API Documentation](https://unity-technologies.github.io/ml-agents/Python-Gym-API-Documentation/)がある。

## Unityの `Project Settings` 重要項目

メニュバーの`Edit` -> `Project Settings...`から開く。

### `Time`

Fixed Timestampを `0.01`に設定する。この値が `Decision Requester`の`Decision Period`の最小周期になる。

例えば `Decision Period`を`10`に設定すると、Python上では10fpsでインタラクションが実行される。

### `Player`

`Resolution and Presentation`の中で、次の項目を必ず設定する。

* `Run In Background`: チェックする。
* `Fullscreen Mode`: `Windowed`

`Standalone Player Options`の`Resizable Window`もチェックしておくとデバッグを行いやすい。
