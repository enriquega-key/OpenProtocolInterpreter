﻿using System.Collections.Generic;

namespace OpenProtocolInterpreter.MotorTuning
{
    /// <summary>
    /// Motor tuning result data unsubscribe
    /// <para>Reset the motor tuning result subscription.</para>
    /// <para>Message sent by: Integrator</para>
    /// <para>Answer: <see cref="Communication.Mid0005"/> Command accepted or <see cref="Communication.Mid0004"/> Command error, Motor Tuning result subscription does not exist</para>
    /// </summary>
    public class Mid0503 : Mid, IMotorTuning, IIntegrator, IUnsubscription, IAcceptableCommand, IDeclinableCommand
    {
        private const int LAST_REVISION = 1;
        public const int MID = 503;

        public IEnumerable<Error> PossibleErrors => new Error[] { Error.motor };

        public Mid0503() : base(MID, LAST_REVISION) { }

        public Mid0503(Header header) : base(header)
        {
        }
    }
}
