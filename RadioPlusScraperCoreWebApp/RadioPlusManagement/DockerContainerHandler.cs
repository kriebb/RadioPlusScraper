using System;

namespace RadioPlusScraperWebApi
{
    internal class DockerContainerHandler : IDockerContainerHandler
    {
        public void Start()
        {
            /* using (DockerClient client =
                new DockerClientConfiguration(new Uri("http://ubuntu-docker.cloudapp.net:4243")).CreateClient())
            {
                var task = client.Containers.StartContainerAsync("39e3317fd258", new ContainerStartParameters());
            
                if (!task.Result)
                    throw new CouldNotStartDockerException();
            }*/
        }

        public void Stop()
        {
            //
        }
    }
}