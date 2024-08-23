using FastEndpoints;

namespace AspireIssueTracker.Users.Api.Endpoints
{
	public class Response
	{
		public string? Message { get; set; }
	}

	public class TestEndpoint() : EndpointWithoutRequest<Response>
	{
		public override void Configure()
		{
			Get("/api");
			Permissions("read:users");
		}

		public override Task<Response> ExecuteAsync(CancellationToken ct)
		{
			var claims = User.Claims.Select(c => $"{c.Type} => ${c.Value}");
			return Task.FromResult(new Response { Message = string.Join(",", claims) ?? "Hello world!" });
		}
	}
}