using System.Configuration;
using System.Drawing;
using System.Linq;
using ImageProcessing.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RainIt.Interfaces.Repository;

namespace RainIt.Business.Tests
{
    [TestClass]
    public class PatternManagerTests : ManagerTests
    {
       
        [TestMethod]
        public void AddPattern_NewValidPattern()
        {
            //Arrange 
            var manager = new PatternManager(RainItContext, AzureCloudContext);

            //Act
            var allImagesBeforeAdd = RainItContext.UserPatternSet.ToList();
            var imageToAdd = new ImageDetails(new Bitmap(200, 200));
            const string fileName = "newimage1";
            var canAdd = manager.AddUserPattern(imageToAdd, fileName);
            var allImagesAfterAdd = RainItContext.UserPatternSet.ToList();

            //Assert
            Assert.IsTrue(!canAdd.IsError, "the image should have been added");
            Assert.AreEqual(allImagesBeforeAdd.Count()+1, allImagesAfterAdd.Count(), "the image was not added");
            Assert.IsNotNull(RainItContext.UserPatternSet.Single(up => up.Name == fileName), "The image was not correctly added");
        }

        [TestMethod]
        public void AddPattern_InvalidDimentions()
        {
            //Arrange 
            var manager = new PatternManager(RainItContext, AzureCloudContext);

            //Act
            var allImagesBeforeAdd = RainItContext.UserPatternSet.ToList();
            var imageToAdd = new ImageDetails(new Bitmap(300, 100));
            const string fileName = "newimage1";
            var canAdd = manager.AddUserPattern(imageToAdd, fileName);
            var allImagesAfterAdd = RainItContext.UserPatternSet.ToList();

            //Assert
            Assert.IsFalse(!canAdd.IsError, "the image should have not been added");
            Assert.AreEqual(allImagesBeforeAdd.Count(), allImagesAfterAdd.Count(), "the image was added");
            Assert.IsNull(RainItContext.UserPatternSet.SingleOrDefault(up => up.Name == fileName), "The image was correctly added");
        }

        [TestMethod]
        public void AddPattern_InvalidExistingPattern()
        {
            //Arrange 
            var manager = new PatternManager(RainItContext, AzureCloudContext);

            //Act
            var allImagesBeforeAdd = RainItContext.UserPatternSet.ToList();
            var imageToAdd = new ImageDetails(new Bitmap(200, 200));
            const string fileName = "samplepattern1";
            var canAdd = manager.AddUserPattern(imageToAdd, fileName);
            var allImagesAfterAdd = RainItContext.UserPatternSet.ToList();

            //Assert
            Assert.IsFalse(!canAdd.IsError, " the image should not have been added");
            Assert.AreEqual(allImagesBeforeAdd.Count(), allImagesAfterAdd.Count(), "the image was added");
        }

        [TestMethod]
        public void UpdatePattern_ExistingPattern()
        {
            //Arrange 
            var manager = new PatternManager(RainItContext, AzureCloudContext);

            //Act
            var allImagesBeforeUpdate = RainItContext.UserPatternSet.ToList();
            const string fileName = "samplepattern1";
            var imageToAdd = new ImageDetails(new Bitmap(200, 200));
            var canUpdate = manager.UpdateUserPattern(imageToAdd, fileName);
            var allImagesAfterUpdate = RainItContext.UserPatternSet.ToList();
            var afterUpdateImage = RainItContext.UserPatternSet.Single(p => p.Name == fileName);

            //Assert
            Assert.IsTrue(!canUpdate.IsError, " the image should have been updated");
            Assert.AreEqual(allImagesBeforeUpdate.Count(), allImagesAfterUpdate.Count(), "an image was added");
            Assert.AreEqual(200, afterUpdateImage.Height, "the height was not updated");
            Assert.AreEqual(200, afterUpdateImage.Width, "the height was not updated");
        }

        [TestMethod]
        public void UpdatePattern_NonExistingPattern()
        {
            //Arrange 
            var manager = new PatternManager(RainItContext, AzureCloudContext);

            //Act
            var allImagesBeforeUpdate = RainItContext.UserPatternSet.ToList();
            var imageToAdd = new ImageDetails(new Bitmap(200, 200));
            const string fileName = "newpattern1";
            var canUpdate = manager.UpdateUserPattern(imageToAdd, fileName);
            var allImagesAfterUpdate = RainItContext.UserPatternSet.ToList();

            //Assert
            Assert.IsTrue(!canUpdate.IsError, " the image should have been updated");
            Assert.AreEqual(allImagesBeforeUpdate.Count()+1, allImagesAfterUpdate.Count(), "the  image was not  updated");
            Assert.IsNotNull(RainItContext.UserPatternSet.Single(up => up.Name == fileName), "The image was not correctly added");
        }

        [TestMethod]
        public void DeletePattern_ExistingPattern()
        {
            //Arrange 
            var manager = new PatternManager(RainItContext, AzureCloudContext);

            //Act 
            var allImagesBeforeDelete = RainItContext.UserPatternSet.ToList();
            const string fileName = "samplepattern1";
            var canDelete = manager.DeleteUserPattern(fileName);
            var allImagesAfterDelete = RainItContext.UserPatternSet.ToList();

            //Assert
            Assert.IsTrue(!canDelete.IsError, "the image should have been deleted");
            Assert.AreEqual(allImagesAfterDelete.Count()-1, allImagesBeforeDelete.Count(), "the image was not deleted");
            Assert.IsNull(RainItContext.UserPatternSet.SingleOrDefault(p => p.Name == fileName), " the image was not deleted");

        }

        [TestMethod]
        public void DeletePattern_NonExistingPattern()
        {
            //Arrange 
            var manager = new PatternManager(RainItContext, AzureCloudContext);

            //Act 
            var allImagesBeforeDelete = RainItContext.UserPatternSet.ToList();
            const string fileName = "otherpattern";
            var canDelete = manager.DeleteUserPattern(fileName);
            var allImagesAfterDelete = RainItContext.UserPatternSet.ToList();

            //Assert
            Assert.IsFalse(!canDelete.IsError, "the image should not have been deleted");
            Assert.AreEqual(allImagesAfterDelete.Count(), allImagesBeforeDelete.Count(), "the image was not deleted");

        }

        [TestMethod]
        public void GetAllUserPatterns()
        {
            //Arrange 
            var manager = new PatternManager(RainItContext, AzureCloudContext);

            //Act 
            var userPatterns = manager.GetUserPatterns();

            //Assert
            Assert.IsNotNull(userPatterns, " no user patterns were retrieved");
            Assert.AreEqual(userPatterns.Count(), RainItContext.UserPatternSet.Count(), "the number of patterns retrieved is incorrect");
        }
        
    }
}
