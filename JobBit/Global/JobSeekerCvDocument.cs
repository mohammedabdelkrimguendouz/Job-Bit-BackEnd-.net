using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using static JobBit_Business.JobSeeker;

namespace JobBit.Global
{
    public class JobSeekerCvDocument : IDocument
    {
        private readonly AllJobSeekerInfo _info;

        public JobSeekerCvDocument(AllJobSeekerInfo info)
        {
            _info = info;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken3));

                page.Content().Column(col =>
                {
                    ComposeHeader(col);
                    ComposeSummary(col);
                    ComposeSkills(col);
                    ComposeCertifications(col);
                    ComposeEducation(col);
                    ComposeFooter(col);
                });
            });
        }

        private void ComposeHeader(ColumnDescriptor col)
        {
            col.Item().Row(row =>
            {
                if (!string.IsNullOrEmpty(_info.ProfilePicturePath) && File.Exists(_info.ProfilePicturePath))
                {
                    row.RelativeItem(1).MinWidth(100).MinHeight(100).MaxWidth(100).MaxHeight(100)
                        .Background(Colors.White)
                        .Border(1)
                        .AlignCenter()
                        .AlignMiddle()
                        .Image(_info.ProfilePicturePath, ImageScaling.FitArea);
                }
                else
                {
                    row.RelativeItem(1).MinWidth(100).MinHeight(100).MaxWidth(100).MaxHeight(100)
                        .Background(Colors.White)
                        .Border(1)
                        .AlignCenter()
                        .AlignMiddle()
                        .Text("Photo");
                }

                row.RelativeItem(4).Padding(10).Column(header =>
                {
                    header.Item().Text($"{_info.FirstName} {_info.LastName}")
                        .FontSize(24).Bold().FontColor(Colors.Black);

                    header.Item().Text("Full Stack Developer | Specialized in Desktop Applications and Backend Systems | Eager to Learn and Build Impactful Solutions")
                        .FontSize(12).FontColor(Colors.Grey.Darken2);

                    if (_info.WilayaInfo != null)
                    {
                        header.Item().Text($"{_info.WilayaInfo.Name}, Algeria")
                            .FontSize(11).FontColor(Colors.Grey.Darken2);
                    }

                    header.Item().PaddingTop(10);

                    // قسم الاتصال
                    header.Item().Background(Colors.Grey.Lighten3).Padding(10).Column(contact =>
                    {
                        contact.Item().Text("Contact").Bold().FontSize(12);

                        contact.Item().Grid(grid =>
                        {
                            grid.Columns(2);
                            grid.Spacing(5);

                            grid.Item().Text($"📱 {_info.Phone}").FontColor(Colors.Grey.Darken1);
                            grid.Item().Text($"✉️ {_info.Email}").FontColor(Colors.Grey.Darken1);

                            if (!string.IsNullOrEmpty(_info.LinkProfileLinkden))
                                grid.Item().Column(c => c.Item().Text(text =>
                                {
                                    text.Span("🔗 LinkedIn: ");
                                    text.Span(_info.LinkProfileLinkden)
                                        .FontColor(Colors.Blue.Medium)
                                        .Underline()
                                        .WrapAnywhere();
                                }));

                            if (!string.IsNullOrEmpty(_info.LinkProfileGithub))
                                grid.Item().Column(c => c.Item().Text(text =>
                                {
                                    text.Span("💻 GitHub: ");
                                    text.Span(_info.LinkProfileGithub)
                                        .FontColor(Colors.Blue.Medium)
                                        .Underline()
                                        .WrapAnywhere();
                                }));
                        });
                    });
                });
            });

            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        }

        private void ComposeSummary(ColumnDescriptor col)
        {
            col.Item().Text("Summary").Bold().FontSize(16);
            col.Item().PaddingTop(5).Text("Curiosity fuels creation. From a young age, I was fascinated by how technology shapes the world. That passion led me to pursue software development—where I transform real-life challenges into smart, scalable solutions.")
                .FontSize(11);

            col.Item().PaddingTop(10).Text("I'm a Full Stack Developer specialized in Desktop Applications and Backend Systems, with a strong foundation in C#, .NET Framework/Core, and SQL Server. I build structured and efficient solutions using multi-layered architecture and RESTful APIs.")
                .FontSize(11);

            col.Item().PaddingTop(10).Text("Some of my achievements include:")
                .FontSize(11).Bold();

            col.Item().PaddingLeft(15).PaddingTop(5).Text("• Independently developing three full-featured desktop systems (Library, Hotel, and Driver License Management).")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• Automating real-life services like driving tests, hotel reservations, and membership tracking.")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• Creating a backend code generator that builds CRUD logic for Business Layer, Data Access Layer, and SQL Procedures.")
                .FontSize(11);

            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        }

        private void ComposeSkills(ColumnDescriptor col)
        {
            col.Item().Text("Key Skills").Bold().FontSize(16);

            col.Item().PaddingTop(5).Text("Programming: C#, C++, SQL")
                .FontSize(11);
            col.Item().Text("Technologies: .NET Framework, .NET Core, ADO.NET, RESTful APIs")
                .FontSize(11);
            col.Item().Text("Architecture: 3-Tier Architecture, UI/UX-focused designs")
                .FontSize(11);
            col.Item().Text("Tools: Visual Studio, SQL Server")
                .FontSize(11);

            col.Item().PaddingTop(10).Text("Top Skills").Bold().FontSize(14);

            if (_info.Skills?.Count > 0)
            {
                col.Item().PaddingTop(5).Grid(grid =>
                {
                    grid.Columns(3);
                    grid.Spacing(5);

                    foreach (var skill in _info.Skills)
                    {
                        grid.Item().MinWidth(100).Padding(2).Row(row =>
                        {
                            row.AutoItem().Text("• ").FontColor(Colors.Blue.Medium);
                            row.RelativeItem().Text(skill.Name).FontColor(Colors.Grey.Darken2).FontSize(11);
                        });
                    }
                });
            }

            col.Item().PaddingTop(10).Text("I can help you with:").Bold().FontSize(14);

            col.Item().PaddingLeft(15).PaddingTop(5).Text("• Developing custom desktop software")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• Designing robust backend systems and APIs")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• Optimizing databases and data access layers")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• Turning your software ideas into fully functional applications")
                .FontSize(11);

            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        }

        private void ComposeCertifications(ColumnDescriptor col)
        {
            col.Item().Text("Certifications").Bold().FontSize(16);

            col.Item().PaddingLeft(15).PaddingTop(5).Text("• Algorithms & Problem-Solving Level 2")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• Database - SQL (Projects & Practice)")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• Algorithms & Problem-Solving Level 1 Solutions")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• Programming Using C++ - Level 1")
                .FontSize(11);
            col.Item().PaddingLeft(15).Text("• OOP As It Should Be In C#")
                .FontSize(11);

            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        }

        private void ComposeEducation(ColumnDescriptor col)
        {
            col.Item().Text("Education").Bold().FontSize(16);

            col.Item().PaddingLeft(15).PaddingTop(5).Text("Kasdi Merbah University - Ouargla (2022 - 2025)")
                .FontSize(11);

            col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
        }

        private void ComposeFooter(ColumnDescriptor col)
        {
            col.Item().PaddingTop(20).AlignCenter().Text(text =>
            {
                text.Span("Let's connect! Whether you're a developer, recruiter, or a client with a vision—I'm open to collaboration, learning, and creating. Feel free to message me or send a connection request. Let's build something meaningful together!")
                    .FontSize(10).FontColor(Colors.Grey.Darken2);
            });

            col.Item().PaddingTop(10).AlignCenter().Text(text =>
            {
                text.Span("Generated with ").FontSize(8).FontColor(Colors.Grey.Medium);
                text.Span("JobBit").FontSize(8).Bold().FontColor(Colors.Blue.Medium);
                text.Span(" | Page ").FontSize(8).FontColor(Colors.Grey.Medium);
                text.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                text.Span(" of ").FontSize(8).FontColor(Colors.Grey.Medium);
                text.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
            });
        }
    }
}