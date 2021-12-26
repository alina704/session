/// <summary>
/// Отрисовка карточки водительского удостоверения и отображение её в driverLicenseImage.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void DriverLicensePaintClick(object sender, EventArgs e)
{
    int id = drivers_LicsDataGridView.CurrentCell.RowIndex + 1;
    // Подгрузка данных из БД.
    drivers current = db.drivers.SqlQuery($"select * from drivers where id = {id}").AsNoTracking().FirstOrDefaultAsync().Result;
    licences license = db.licences.SqlQuery($"select * from licences where id = {id}").AsNoTracking().FirstOrDefaultAsync().Result;
    // Отрисовка изображения.
    DriverLicenseImage img = new DriverLicenseImage(Properties.Resources.driver_license_template);
    img.DrawInfo(current.lastname, current.firstname, current.middlename, license.categories, current.address,
                 license.licence_date.ToShortDateString(), license.expire_date.ToShortDateString(), license.licence_number.ToString(), license.licence_series);
    driverLicenseImage.Image = img.GetBitmap();  // Подгрузка в pictureBox.
    driverLicenseSaveImageButton.Enabled = true;
}
/// <summary>
/// Вызывает <see cref="SaveFileDialog"/> для сохранения карточки водителя в файл.
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void DriverLicenseSaveImage(object sender, EventArgs e)
{
    SaveFileDialog dialog = new SaveFileDialog();
    Stream stream;
    dialog.Filter = "image files (*.jpg)|*.jpg";
    dialog.FilterIndex = 2;
    dialog.RestoreDirectory = true;

    if (dialog.ShowDialog() == DialogResult.OK)
    {
        if ((stream = dialog.OpenFile()) != null)
        {
            byte[] data = DriverLicenseImage.GetBytes(driverLicenseImage.Image);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }
    }
}