// Copyright (c) "Neo4j"
// Neo4j Sweden AB [http://neo4j.com]
// 
// This file is part of Neo4j.
// 
// Licensed under the Apache License, Version 2.0 (the "License"):
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Serilog;
using Serilog.Events;
using ILogger = Neo4j.Driver.ILogger;

namespace ReflectionHydration;

public class Neo4jSerilogger : ILogger
{
    /// <inheritdoc />
    public void Error(Exception cause, string message, params object[] args)
    {
        Log.Logger.Error(cause, message, args);
    }

    /// <inheritdoc />
    public void Warn(Exception cause, string message, params object[] args)
    {
        Log.Logger.Warning(cause, message, args);
    }

    /// <inheritdoc />
    public void Info(string message, params object[] args)
    {
        Log.Logger.Information(message, args);
    }

    /// <inheritdoc />
    public void Debug(string message, params object[] args)
    {
        Log.Logger.Debug(message, args);
    }

    /// <inheritdoc />
    public void Trace(string message, params object[] args)
    {
        Log.Logger.Verbose(message, args);
    }

    /// <inheritdoc />
    public bool IsTraceEnabled()
    {
        return Log.Logger.IsEnabled(LogEventLevel.Verbose);
    }

    /// <inheritdoc />
    public bool IsDebugEnabled()
    {
        return Log.Logger.IsEnabled(LogEventLevel.Debug);
    }
}
