﻿/* Copyright 2010-present MongoDB Inc.
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

using MongoDB.Bson;
using MongoDB.Driver.Core.Misc;
using MongoDB.Driver.Linq.Linq3Implementation.Ast.Visitors;
using MongoDB.Driver.Linq.Linq3Implementation.Misc;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB.Driver.Linq.Linq3Implementation.Ast.Expressions
{
    internal sealed class AstNaryExpression : AstExpression
    {
        private readonly IReadOnlyList<AstExpression> _args;
        private readonly AstNaryOperator _operator;

        public AstNaryExpression(AstNaryOperator @operator, IEnumerable<AstExpression> args)
        {
            _operator = @operator;
            _args = Ensure.IsNotNull(args, nameof(args)).AsReadOnlyList();
        }

        public AstNaryExpression(AstNaryOperator @operator, params AstExpression[] args)
            : this(@operator, (IEnumerable<AstExpression>)args)
        {
        }

        public IReadOnlyList<AstExpression> Args => _args;
        public override AstNodeType NodeType => AstNodeType.NaryExpression;
        public AstNaryOperator Operator => _operator;

        public override AstNode Accept(AstNodeVisitor visitor)
        {
            return visitor.VisitNaryExpression(this);
        }

        public override BsonValue Render()
        {
            return new BsonDocument(_operator.Render(), new BsonArray(_args.Select(e => e.Render())));
        }

        public AstNaryExpression Update(IEnumerable<AstExpression> args)
        {
            if (args == _args)
            {
                return this;
            }

            return new AstNaryExpression(_operator, args);
        }
    }
}
