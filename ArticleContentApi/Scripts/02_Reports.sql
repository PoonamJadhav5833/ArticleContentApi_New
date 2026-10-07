-- -Report to show all authors and array of article ids written by them
SELECT
    u.Id,
    u.Username,
    STRING_AGG(CONVERT(VARCHAR(20), x.ArticleId), ',') AS ArticleIds
FROM Users u
LEFT JOIN
(
    SELECT DISTINCT AuthorId, ArticleId
    FROM Contents
) x ON x.AuthorId = u.Id
GROUP BY u.Id, u.Username
ORDER BY u.Username;


--Report to show all articles created in past 3 months, 
--by users created in the past 4months with specified language (+ points for script with variables)

DECLARE @Language VARCHAR(30) = 'English';
DECLARE @AsOfUtc DATETIME = GETUTCDATE();

SELECT
    a.Id AS ArticleId,
    a.CreatedAt AS ArticleCreatedAt,
    a.Status AS ArticleStatus,
    c.Id AS ContentId,
    c.Title,
    c.Language,
    u.Id AS AuthorId,
    u.Username AS Author,
    u.CreatedAt AS AuthorCreatedAt
FROM Articles a
INNER JOIN Contents c
    ON c.ArticleId = a.Id
INNER JOIN Users u
    ON u.Id = c.AuthorId
WHERE a.CreatedAt >= DATEADD(MONTH, -3, @AsOfUtc)
  AND a.CreatedAt <= @AsOfUtc
  AND u.CreatedAt >= DATEADD(MONTH, -4, @AsOfUtc)
  AND u.CreatedAt <= @AsOfUtc
  AND c.Language = @Language
ORDER BY a.CreatedAt DESC, a.Id;
