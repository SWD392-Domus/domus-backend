﻿ALTER TABLE Package
DROP COLUMN IsDeleted
ALTER TABLE Package
ADD IsDeleted BIT DEFAULT 0