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
from mlagents_envs.side_channel.side_channel import (
    SideChannel,
    IncomingMessage,
    OutgoingMessage,
)
import uuid

class TransformLogChannel(SideChannel):
    def __init__(self, id):
        super().__init__(id)

    def on_message_received(self, msg: IncomingMessage):
        value_list = msg.read_float32_list()
        frame_count = value_list[0]
        position_x = value_list[1]
        position_y = value_list[2]
        position_z = value_list[3]
        euler_x = value_list[4]
        euler_y = value_list[5]
        euler_z = value_list[6]
        format_string = f"{frame_count}, {position_x}, {position_y}, {position_z}, {euler_x}, {euler_y}, {euler_z}"
        print(format_string)

def main():
    if len(sys.argv) < 2:
        print("使用法: python script_name.py <unity_env_file_path>")
        sys.exit(1)

    unity_env_file_path = sys.argv[1]
    print(f"Unity環境ファイルパス: {unity_env_file_path}")

    engine_channel = EngineConfigurationChannel()
    transform_log_channel = TransformLogChannel(uuid.UUID("621f0a70-4f87-11ea-a6bf-784f4387d1f7"))
    env = UnityToGymWrapper(
        UnityEnvironment(
            unity_env_file_path,
            side_channels=[engine_channel, transform_log_channel],
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