﻿// <copyright file="LineProtocolPayloadTests.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using App.Metrics.Formatting.InfluxDB;
using FluentAssertions;
using Xunit;

namespace App.Metrics.Extensions.Reporting.InfluxDB.Facts.Client
{
    public class LineProtocolPayloadTests
    {
        [Fact]
        public void Can_format_payload()
        {
            var textWriter = new StringWriter();
            var payload = new LineProtocolPayload();
            var fieldsOne = new Dictionary<string, object> { { "key", "value" } };
            var timestampOne = new DateTime(2017, 1, 1, 1, 1, 1, DateTimeKind.Utc);
            var pointOne = new LineProtocolPoint("measurement", fieldsOne, MetricTags.Empty, timestampOne);

            var fieldsTwo = new Dictionary<string, object>
                            {
                                { "field1key", "field1value" },
                                { "field2key", 2 },
                                { "field3key", false }
                            };
            var timestampTwo = new DateTime(2017, 1, 2, 1, 1, 1, DateTimeKind.Utc);
            var pointTwo = new LineProtocolPoint("measurement", fieldsTwo, MetricTags.Empty, timestampTwo);

            payload.Add(pointOne);
            payload.Add(pointTwo);

            payload.Format(textWriter);

            textWriter.ToString().Should().Be(
                "measurement key=\"value\" 1483232461000000000\nmeasurement field1key=\"field1value\",field2key=2i,field3key=f 1483318861000000000\n");
        }

        [Fact]
        public void When_null_point_ignore_and_dont_throw()
        {
            var payload = new LineProtocolPayload();

            Action action = () => { payload.Add(null); };

            action.ShouldNotThrow();
        }

        [Fact]
        public void When_null_text_writer_ignore_and_dont_throw()
        {
            var payload = new LineProtocolPayload();
            var fields = new Dictionary<string, object> { { "key", "value" } };
            var pointOne = new LineProtocolPoint("measurement", fields, MetricTags.Empty);

            Action action = () =>
            {
                payload.Add(pointOne);
                payload.Format(null);
            };

            action.ShouldNotThrow();
        }
    }
}