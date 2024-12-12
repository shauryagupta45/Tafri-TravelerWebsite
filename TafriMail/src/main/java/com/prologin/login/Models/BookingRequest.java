package com.prologin.login.Models;

import java.sql.Timestamp;
import java.util.List;
import java.util.Map;

public class BookingRequest {
    private String UserName;
    private String UserEmail;
    private String SupplierName;
    private String SupplierContact;
    private String PackageName;
    private String Source;
    private String Destination;
    private Timestamp StartDatetime;
    private Integer Duration;
    private Integer PaymentId;
    private Integer PaymentAmount;
    private String PaymentMode;

    public String getUserName() {
        return UserName;
    }

    public void setUserName(String userName) {
        UserName = userName;
    }

    public String getUserEmail() {
        return UserEmail;
    }

    public void setUserEmail(String userEmail) {
        UserEmail = userEmail;
    }

    public String getSupplierName() {
        return SupplierName;
    }

    public void setSupplierName(String supplierName) {
        SupplierName = supplierName;
    }

    public String getSupplierContact() {
        return SupplierContact;
    }

    public void setSupplierContact(String supplierContact) {
        SupplierContact = supplierContact;
    }

    public String getPackageName() {
        return PackageName;
    }

    public void setPackageName(String packageName) {
        PackageName = packageName;
    }

    public String getSource() {
        return Source;
    }

    public void setSource(String source) {
        Source = source;
    }

    public String getDestination() {
        return Destination;
    }

    public void setDestination(String destination) {
        Destination = destination;
    }

    public Timestamp getStartDatetime() {
        return StartDatetime;
    }

    public void setStartDatetime(Timestamp startDatetime) {
        StartDatetime = startDatetime;
    }

    public Integer getDuration() {
        return Duration;
    }

    public void setDuration(Integer duration) {
        Duration = duration;
    }

    public Integer getPaymentId() {
        return PaymentId;
    }

    public void setPaymentId(Integer paymentId) {
        PaymentId = paymentId;
    }

    public Integer getPaymentAmount() {
        return PaymentAmount;
    }

    public void setPaymentAmount(Integer paymentAmount) {
        PaymentAmount = paymentAmount;
    }

    public String getPaymentMode() {
        return PaymentMode;
    }

    public void setPaymentMode(String paymentMode) {
        PaymentMode = paymentMode;
    }
}
