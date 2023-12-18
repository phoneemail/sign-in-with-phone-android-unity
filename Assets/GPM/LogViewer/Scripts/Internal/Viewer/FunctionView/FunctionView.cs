﻿namespace Gpm.LogViewer.Internal
{
    using System.Collections.Generic;

    public class FunctionView : ViewBase
    {
        public CommandList commandList = null;

        private int showCommandCount = 0;

        public void InputCheatKey(string cheatKey)
        {
            Function.Instance.InvokeCheatKey(cheatKey);
        }

        private void Update()
        {
            List<Function.CommandData> commands = Function.Instance.GetCommandDatas();

            if (showCommandCount < commands.Count)
            {
                for (int index = showCommandCount; index < commands.Count; ++index)
                {
                    commandList.Insert(commands[index]);
                    ++showCommandCount;
                }
            }
        }
    }
}