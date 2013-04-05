using Machine.Specifications;
using nget.core.Fs;
using nget.specs.TestDoubles;

namespace nget.specs
{
    public class With_a_FakeFileSystem
    {
        Establish context = () => { fakeFileSystem = new FakeFileSystem(); };

        protected static IFileSystem fakeFileSystem;
    }
}