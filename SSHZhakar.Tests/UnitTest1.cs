using NUnit.Framework;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SSHZhakar;
using FrontEnd.Controllers;
using Microsoft.AspNetCore.Mvc;
using SSHZHAKAR.Controllers;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;

namespace SSHZhakar.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task IsPhotoUploaded_FileWasCreated_ReturnTrue()
        {

            UserController userController = new UserController();
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo(@"C:\Users\Alexander\Pictures\WOLK.png");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;

            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));


            var sut = new UserController();
            var file = fileMock.Object;
            List<IFormFile> mylist = new List<IFormFile>();
            mylist.Add(file);
            var a = await userController.onPhotoUpload(mylist, 123);
            
            FileAssert.Exists(@"C:\Users\Alexander\source\repos\SSHZHAKAR\SSHZHAKAR\wwwroot\static\123.png");

        }

        [Test]
        public async Task IsPhotoUploaded_FileWasCreated_ReturnTrue1()
        {

            UserController userController = new UserController();
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo(@"C:\Users\Alexander\Pictures\WOLK.png");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;

            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));


            var sut = new UserController();
            var file = fileMock.Object;
            List<IFormFile> mylist = new List<IFormFile>();
            mylist.Add(file);
            var a = await userController.onPhotoUpload(mylist, 35126126);
            
            FileAssert.Exists(@"C:\Users\Alexander\source\repos\SSHZHAKAR\SSHZHAKAR\wwwroot\static\35126126.png");

        }
        [Test]
        public async Task IsPhotoUploaded_FileWasCreated_ReturnTrue2()
        {

            UserController userController = new UserController();
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo(@"C:\Users\Alexander\Pictures\WOLK.png");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;

            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));


            var sut = new UserController();
            var file = fileMock.Object;
            List<IFormFile> mylist = new List<IFormFile>();
            mylist.Add(file);
            var a = await userController.onPhotoUpload(mylist, 26);
            
            FileAssert.Exists(@"C:\Users\Alexander\source\repos\SSHZHAKAR\SSHZHAKAR\wwwroot\static\26.png");

        }

        [Test]
        public async Task IsPhotoUploaded_FileWasCreated_ReturnTrue3()
        {

            UserController userController = new UserController();
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo(@"C:\Users\Alexander\Pictures\WOLK.png");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;

            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));


            var sut = new UserController();
            var file = fileMock.Object;
            List<IFormFile> mylist = new List<IFormFile>();
            mylist.Add(file);
            var a = await userController.onPhotoUpload(mylist, 1111111);
            
            FileAssert.Exists(@"C:\Users\Alexander\source\repos\SSHZHAKAR\SSHZHAKAR\wwwroot\static\1111111.png");

        }

        [Test]
        public async Task IsPhotoUploaded_FileWasCreated_ReturnTrue4()
        {

            UserController userController = new UserController();
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo(@"C:\Users\Alexander\Pictures\WOLK.png");
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            writer.Flush();
            ms.Position = 0;
            var fileName = physicalFile.Name;

            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));


            var sut = new UserController();
            var file = fileMock.Object;
            List<IFormFile> mylist = new List<IFormFile>();
            mylist.Add(file);
            var a = await userController.onPhotoUpload(mylist, 14886);
            
            FileAssert.Exists(@"C:\Users\Alexander\source\repos\SSHZHAKAR\SSHZHAKAR\wwwroot\static\14886.png");

        }

        [Test]
        public void IsResponseCorrect_CommandResponseCorrect_ReturnTrue()
        {
            SSH_USER_CORE.User user = new SSH_USER_CORE.User();
            user.CreateSSHConnection("192.168.1.6", "alexander", "123321sanek");
            user.SSHConnect();
            string result = user.SSHRunCommand("ls");
            user.SSHDisconnect();
            Assert.AreEqual(result, "alexanderfolder\nalexander.txt\ntexts\n");

        }
        [Test]
        public void IsResponseCorrect_CommandResponseCorrect_ReturnTrue1()
        {
            SSH_USER_CORE.User user = new SSH_USER_CORE.User();
            user.CreateSSHConnection("192.168.1.6", "alexander", "123321sanek");
            user.SSHConnect();
            string result = user.SSHRunCommand("whoami");
            user.SSHDisconnect();
            Assert.AreEqual(result, "alexander\n");

        }

        [Test]
        public void IsResponseCorrect_CommandResponseCorrect_ReturnTrue2()
        {
            SSH_USER_CORE.User user = new SSH_USER_CORE.User();
            user.CreateSSHConnection("192.168.1.6", "alexander", "123321sanek");
            user.SSHConnect();
            string result = user.SSHRunCommand("cat alexander.txt");
            user.SSHDisconnect();
            Assert.AreEqual(result, "some text\n");

        }

        [Test]
        public void IsResponseCorrect_CommandResponseCorrect_ReturnTrue3()
        {
            SSH_USER_CORE.User user = new SSH_USER_CORE.User();
            user.CreateSSHConnection("192.168.1.6", "alexander", "123321sanek");
            user.SSHConnect();
            string result1 = user.SSHRunCommand("umask");
            user.SSHDisconnect();
            Console.WriteLine("Result  = " + result1);
            Assert.AreEqual(result1, "0002\n");
        }
    }
}