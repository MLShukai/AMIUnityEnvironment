"""
実行方法:
python unity_mlagents_gym_interaction.py <unity_env_file_path>

依存関係:
- mlagents-envs

インストール方法:
pip install mlagents-envs

注意:
- Unity MLAgents Environmentが必要です。
- スクリプトを実行する前に、Unityプロジェクトをビルドし、実行可能ファイルのパスを引数として渡してください。
"""

import sys
import time
from mlagents_envs.environment import UnityEnvironment
from mlagents_envs.envs.unity_gym_env import UnityToGymWrapper
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

def main():
    if len(sys.argv) < 2:
        print("使用法: python script_name.py <unity_env_file_path>")
        sys.exit(1)

    unity_env_file_path = sys.argv[1]
    print(f"Unity環境ファイルパス: {unity_env_file_path}")

    engine_channel = EngineConfigurationChannel()
    env = UnityToGymWrapper(
        UnityEnvironment(
            unity_env_file_path,
            side_channels=[engine_channel],
            no_graphics=False,
            worker_id=0
        )
    )
    engine_channel.set_configuration_parameters(time_scale=1.0, target_frame_rate=60)

    print("Action Space: ", env.action_space)
    print("Observation Space:", env.observation_space)

    for i in range(100):
        start_time = time.perf_counter()

        obs, reward, done, info = env.step(env.action_space.sample())
        if done:
            env.reset()

        elapsed = time.perf_counter() - start_time
        fps = 1 / elapsed
        print(f"Frame {i}: {fps:.2f} FPS, 経過時間: {elapsed:.4f}秒")

    env.close()

if __name__ == "__main__":
    main()