﻿/* Copyright 2015-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using Xunit;

namespace MongoDB.Driver.GridFS.Tests
{
    public class GridFSExceptionTests
    {
        [Fact]
        public void constructor_with_messsage_should_initialize_instance()
        {
            var result = new GridFSException("message");

            result.Message.Should().Be("message");
        }

        [Fact]
        public void constructor_with_messsage_and_innerException_should_initialize_instance()
        {
            var innerException = new Exception();
            var result = new GridFSException("message", innerException);

            result.Message.Should().Be("message");
            result.InnerException.Should().BeSameAs(innerException);
        }

        [Fact]
        public void Serialization_should_work()
        {
            var subject = new GridFSException("message", new Exception("inner"));

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, subject);
                stream.Position = 0;
                var rehydrated = (GridFSException)formatter.Deserialize(stream);

                rehydrated.Message.Should().Be(subject.Message);
                rehydrated.InnerException.Message.Should().Be(subject.InnerException.Message);
            }
        }
    }
}
