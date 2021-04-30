using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

namespace CodeAnalysis
{
    class Program
    {
        private static ArgumentListSyntax CreateSingleArgument(string value)
        {
            return SyntaxFactory.ArgumentList(SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(value)))));
        }

        static SyntaxTree CreateTree()
        {
            var consoleWriteline = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName("Console"),
                SyntaxFactory.IdentifierName("WriteLine"));

            var statement = SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(consoleWriteline)
                             .WithArgumentList(CreateSingleArgument("Hello World")));

            var main = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
                "Main")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                .AddBodyStatements(statement);

            var program = SyntaxFactory.ClassDeclaration("Program").AddMembers(main);

            var rootNamespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("CodingTutorials.Demo"));

            rootNamespace = rootNamespace.AddMembers(program);

            var usingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System"));

            var comment = SyntaxFactory.Comment("// Generated code - do not edit");

            var unit = SyntaxFactory.CompilationUnit()
                                    .AddUsings(usingDirective)
                                    .AddMembers(rootNamespace)
                                    .WithLeadingTrivia(comment);

            return unit.SyntaxTree;
        }

        static void Main()
        {
            SyntaxTree tree = CreateTree();

            Console.WriteLine(tree.GetRoot().NormalizeWhitespace().ToFullString());
        }
    }
}
