ALTER TABLE [Contract]
    ADD FOREIGN KEY (ClientId) REFERENCES [DomusUser](Id)

ALTER TABLE [Contract]
    ADD FOREIGN KEY (ContractorId) REFERENCES [DomusUser](Id)
