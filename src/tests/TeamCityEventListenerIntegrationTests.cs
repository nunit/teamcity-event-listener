// ***********************************************************************
// Copyright (c) 2015 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

namespace NUnit.Engine.Listeners
{
    using System.IO;
    using System.Text;
    using Framework;

    [TestFixture]
    public class TeamCityEventListenerIntegrationTests
    {
        private StringBuilder _output;
        private StringWriter _outputWriter;

        [SetUp]
        public void SetUp()
        {
            _output = new StringBuilder();
            _outputWriter = new StringWriter(_output);
        }

        [TearDown]
        public void TearDown()
        {
            _outputWriter.Dispose();
        }        

        [Test]
        [Ignore("")]
        public void ShouldSendMessages()
        {
            // Given
            var publisher = CreateInstance();
            var lines = File.ReadAllLines(@"C:\Projects\NUnit\aa\aa");
            //var lines = File.ReadAllLines(@"C:\Projects\NUnit\aa\aa");

            // When
            foreach (var message in TestUtil.ConvertToMessages(lines))
            {
                publisher.RegisterMessage(message);
            }
                       

            // Then           
            // ReSharper disable once UnusedVariable
            var messages = _output.ToString();           
        }

        private TeamCityEventListener CreateInstance()
        {
            return new TeamCityEventListener(_outputWriter, new TeamCityInfo()) { RootFlowId = string.Empty };
        }        
    }
}
